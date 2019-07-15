using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Proxy;
using SolidRpc.OpenApi.Model;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Proxy
{
    public class SolidRpcProxyAdvice<TObject, TMethod, TAdvice> : SolidRpcOpenApiAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        public SolidRpcProxyAdvice(ILogger<SolidRpcProxyAdvice<TObject, TMethod, TAdvice>> logger, IMethodBinderStore methodBinderStore, IServiceProvider serviceProvider) {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            MethodBinderStore = methodBinderStore ?? throw new ArgumentNullException(nameof(methodBinderStore));
            HttpMessageHandler = (HttpMessageHandler)serviceProvider.GetService(typeof(HttpMessageHandler));
        }

        public HttpMessageHandler HttpMessageHandler { get; }

        private ILogger Logger { get; }
        public IMethodBinderStore MethodBinderStore { get; }
        public IMethodInfo MethodInfo { get; private set; }

        public void Configure(ISolidRpcProxyConfig config)
        {
            base.Configure(config);
            var openApiConfig = config.OpenApiConfiguration;
            if(config.RootAddress != null)
            {
                var swaggerConf = OpenApiParser.ParseOpenApiSpec(config.OpenApiConfiguration);
                swaggerConf.SetSchemeAndHostAndPort(config.RootAddress);
                openApiConfig = swaggerConf.WriteAsJsonString();
            }
            MethodInfo = MethodBinderStore.GetMethodInfo(openApiConfig, config.InvocationConfiguration.MethodInfo);
        }

        private HttpClient CreateHttpClient()
        {
            if(HttpMessageHandler != null)
            {
                return new HttpClient(HttpMessageHandler);
            }
            else
            {
                return new HttpClient();
            }
        }

        public override async Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            using (var httpClient = CreateHttpClient())
            {
                var httpReq = new HttpRequest();
                await MethodInfo.BindArgumentsAsync(httpReq, invocation.Arguments);

                Logger.LogTrace($"Sending data to {httpReq.Scheme}://{httpReq.HostAndPort}{httpReq.Path}");

                var httpClientReq = new HttpRequestMessage();
                httpReq.CopyTo(httpClientReq);

                var httpClientResponse = await httpClient.SendAsync(httpClientReq);
                var httpResp = new HttpResponse();
                await httpResp.CopyFrom(httpClientResponse);

                return MethodInfo.ExtractResponse<TAdvice>(httpResp);
            }
        }
    }
}
