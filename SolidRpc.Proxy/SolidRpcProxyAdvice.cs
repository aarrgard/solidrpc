using SolidProxy.Core.Proxy;
using SolidRpc.Swagger.Binder;
using SolidRpc.Swagger.Model;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolidRpc.Proxy
{
    public class SolidRpcProxyAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        public IMethodInfo MethodInfo { get; private set; }

        public void Configure(ISolidRpcProxyConfig config)
        {
            if(config.SwaggerConfiguration == null)
            {
                throw new Exception($"Solid proxy advice config does not contain a swagger spec for {typeof(TObject)}.");
            }
            // use the swagger binder to setup the invocation
            var swaggerConf = SwaggerParser.ParseSwaggerSpec(config.SwaggerConfiguration);
            if(swaggerConf == null)
            {
                throw new Exception($"Cannot parse swagger configuration({config}).");
            }
            MethodInfo = swaggerConf.GetMethodBinder().GetMethodInfo(config.InvocationConfiguration.MethodInfo);
        }

        public async Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            using (var httpClient = new HttpClient())
            {
                var httpClientRequestMessage = new HttpClientRequestMessage();
                MethodInfo.BindArguments(httpClientRequestMessage, invocation.Arguments);
                var httpClientResponse = await httpClient.SendAsync(httpClientRequestMessage.HttpRequestMessage);
                return await MethodInfo.GetResponse<TAdvice>(new HttpClientResponseMessage(httpClientResponse));
            }
        }
    }
}
