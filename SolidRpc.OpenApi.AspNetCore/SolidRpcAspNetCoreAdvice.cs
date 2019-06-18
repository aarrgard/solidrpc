using SolidProxy.Core.Proxy;
using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.Model;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AspNetCore
{
    public class SolidRpcAspNetCoreAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        public IMethodInfo MethodInfo { get; private set; }

        public void Configure(ISolidRpcAspNetCoreConfig config)
        {
            if(config.OpenApiConfiguration == null)
            {
                throw new Exception($"Solid proxy advice config does not contain a swagger spec for {typeof(TObject)}.");
            }
            // use the swagger binder to setup the invocation
            var swaggerConf = OpenApiParser.ParseSwaggerSpec(config.OpenApiConfiguration);
            if(swaggerConf == null)
            {
                throw new Exception($"Cannot parse swagger configuration({config}).");
            }
            MethodInfo = swaggerConf.GetMethodBinder().GetMethodInfo(config.InvocationConfiguration.MethodInfo);
        }

        /// <summary>
        /// Handles the invocation
        /// </summary>
        /// <param name="next"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public async Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            return default(TAdvice);
        }
    }
}
