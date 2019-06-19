using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SolidProxy.Core.Configuration.Builder;
using SolidProxy.Core.Configuration.Runtime;
using SolidProxy.Core.Proxy;
using SolidRpc.OpenApi.AspNetCore;
using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Binds all the solid rpc proxies that has an implementation on this server.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSolidRpcProxies(this IApplicationBuilder applicationBuilder)
        {
            var runtime = applicationBuilder.ApplicationServices.GetService<ISolidProxyConfigurationStore>();
            if (runtime == null)
            {
                throw new Exception("No solid proxy configuration registered - please configure during startup.");
            }
            runtime.SolidConfigurationBuilder.AssemblyBuilders
                .SelectMany(o => o.Interfaces)
                .SelectMany(o => o.Methods)
                .Where(o => o.IsAdviceConfigured<ISolidRpcAspNetCoreConfig>())
                .ToList()
                .ForEach(o =>
                {
                    var config = o.ConfigureAdvice<ISolidRpcAspNetCoreConfig>();
                    var openApiSpec = OpenApiParser.ParseSwaggerSpec(config.OpenApiConfiguration);
                    var methodBinder = openApiSpec.GetMethodBinder();
                    var methodInfo = methodBinder.GetMethodInfo(o.MethodInfo);
                    BindPath(applicationBuilder, methodInfo.Path, methodInfo);
                });
            return applicationBuilder;
        }

        private static void BindPath(IApplicationBuilder applicationBuilder, string path, IMethodInfo methodInfo)
        {
            applicationBuilder.Run(ctx => HandleInvocation(methodInfo, ctx));
            if (string.IsNullOrEmpty(path))
            {
                applicationBuilder.Run(ctx => HandleInvocation(methodInfo, ctx));
                return;
            }
            if(!path.StartsWith("/"))
            {
                throw new ArgumentException("path must start with '/'");
            }
            var pathPart = path;
            var slashIdx = path.IndexOf('/', 1);
            if (slashIdx == -1)
            {
                slashIdx = path.Length;
            }
            else
            {
                pathPart = path.Substring(0, slashIdx);
            }
            applicationBuilder.Map(pathPart, (ab) => BindPath(ab, path.Substring(slashIdx), methodInfo));
        }

        private static async Task HandleInvocation(IMethodInfo methodInfo, HttpContext context)
        {
            try
            {
                // extract information from http context.
                var request = new SolidRpc.OpenApi.Binder.HttpRequest();
                await request.CopyFrom(context.Request);
                var args = await methodInfo.ExtractArgumentsAsync(request);

                // invoke
                var proxy = context.RequestServices.GetService(methodInfo.MethodInfo.DeclaringType);
                var solidProxy = (ISolidProxy)proxy;
                var res = await solidProxy.InvokeAsync(methodInfo.MethodInfo, args);

                // return response
            }
            catch(Exception e)
            {
                // handle exception
                throw;
            }
        }
    }
}
