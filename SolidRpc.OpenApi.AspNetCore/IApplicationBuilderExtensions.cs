using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SolidProxy.Core.Configuration.Runtime;
using SolidRpc.OpenApi.AspNetCore;
using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.Model;
using System;
using System.Collections.Generic;
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

            //
            // if we map the same path twice we remove old mappings. Extract all paths
            // and map them accordingly.
            //
            var dict = new Dictionary<string, IMethodInfo>();
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
                    dict[$"{methodInfo.Method}{methodInfo.Path}"] = methodInfo;
                });

            //
            // start mapping paths
            //
            foreach(var method in dict.Keys.Select(o => o.Split('/')[0]).Distinct())
            {
                applicationBuilder.MapWhen(
                    ctx => string.Equals(ctx.Request.Method, method, StringComparison.InvariantCultureIgnoreCase),
                    (ab) => BindPath(ab, method, dict));
            }
            return applicationBuilder;
        }

        private static void BindPath(IApplicationBuilder ab, string pathPrefix, Dictionary<string, IMethodInfo> paths)
        {
            //ab.ApplicationServices.LogInformation<IApplicationBuilder>($"Handling path {pathPrefix}");

            // drill down in sub paths
            var parts = paths.Select(o => o.Key)
                .Where(o => o.StartsWith($"{pathPrefix}/"))
                .Select(o => o.Substring(pathPrefix.Length+1).Split('/')[0])
                .Distinct()
                .ToList();

            var fixedPaths = parts.Where(o => !o.StartsWith("{")).Select(o => $"/{o}").ToList();
            foreach (var part in parts)
            {
                ab.MapWhen(ctx => IsMatch(ctx, $"/{part}", fixedPaths), (sab) => BindPath(sab, $"{pathPrefix}/{part}", paths));
            }

            // add handler for this path
            IMethodInfo methodInfo;
            if (paths.TryGetValue(pathPrefix, out methodInfo))
            {
                ab.ApplicationServices.LogInformation<IApplicationBuilder>($"Binding path {pathPrefix} to {methodInfo.OperationId}:{methodInfo.MethodInfo}");
                ab.Run((ctx) => HandleInvocation(methodInfo, ctx));
            }
        }

        private static bool IsMatch(HttpContext ctx, string segment, IEnumerable<string> fixedPaths)
        {
            var path = ctx.Request.Path.Value;
            if (!path.StartsWith("/"))
            {
                return false;
            }
            var nextSlashIdx = path.IndexOf('/', 1);
            if(nextSlashIdx > -1)
            {
                path = path.Substring(0, nextSlashIdx);
            }
            if(path == segment)
            {
                ctx.Request.Path = ctx.Request.Path.Value.Substring(path.Length);
                ctx.Request.PathBase = ctx.Request.PathBase.Add(path);
                return true;
            }
            if(fixedPaths.Contains(path))
            {
                return false;
            }
            if(segment.StartsWith("/{"))
            {
                ctx.Request.Path = ctx.Request.Path.Value.Substring(path.Length);
                ctx.Request.PathBase = ctx.Request.PathBase.Add(path);
                return true;
            }
            return false;
        }

        private static async Task HandleInvocation(IMethodInfo methodInfo, HttpContext context)
        {
            try
            {
                //
                // check if the request is intended for this path.
                //
                if (context.Request.Path != "")
                {
                    return;
                }

                context.RequestServices.LogTrace<IApplicationBuilder>($"Letting {methodInfo.OperationId}:{methodInfo.MethodInfo} handle invocation to {context.Request.Method}:{context.Request.PathBase}{context.Request.Path}");

                // extract information from http context.
                var request = new SolidRpc.OpenApi.Binder.HttpRequest();
                await request.CopyFromAsync(context.Request);

                var methodInvoker = context.RequestServices.GetRequiredService<IMethodInvoker>();
                var response = await methodInvoker.InvokeAsync(request, methodInfo, context.RequestAborted);

                // send data back
                await response.CopyToAsync(context.Response);
            }
            catch (Exception e)
            {
                context.RequestServices.LogError<IApplicationBuilder>(e, "Failed to invoke service");
                throw;
            }
        }
    }
}
