using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Proxy;
using SolidRpc.OpenApi.Model;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Proxy
{
    /// <summary>
    /// The advice that does the invocation on the implementation.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TMethod"></typeparam>
    /// <typeparam name="TAdvice"></typeparam>
    public class SolidRpcProxyAdvice<TObject, TMethod, TAdvice> : SolidRpcOpenApiAdvice<TObject, TMethod, TAdvice> where TObject : class
    {

        /// <summary>
        /// Constucts a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="serviceProvider"></param>
        public SolidRpcProxyAdvice(ILogger<SolidRpcProxyAdvice<TObject, TMethod, TAdvice>> logger, IMethodBinderStore methodBinderStore, IServiceProvider serviceProvider) {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            MethodBinderStore = methodBinderStore ?? throw new ArgumentNullException(nameof(methodBinderStore));
            HttpMessageHandler = (HttpMessageHandler)serviceProvider.GetService(typeof(HttpMessageHandler));
        }

        /// <summary>
        /// The message handler
        /// </summary>
        public HttpMessageHandler HttpMessageHandler { get; }

        /// <summary>
        /// The logger;
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// The binder store
        /// </summary>
        public IMethodBinderStore MethodBinderStore { get; }

        /// <summary>
        /// The method info
        /// </summary>
        public IMethodInfo MethodInfo { get; private set; }

        /// <summary>
        /// Confugures the proxy
        /// </summary>
        /// <param name="config"></param>
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

        /// <summary>
        /// Handles  the invocation
        /// </summary>
        /// <param name="next"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public override async Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            using (var httpClient = CreateHttpClient())
            {
                var httpReq = new SolidHttpRequest();
                await MethodInfo.BindArgumentsAsync(httpReq, invocation.Arguments);

                Logger.LogTrace($"Sending data to {httpReq.Scheme}://{httpReq.HostAndPort}{httpReq.Path}");

                var httpClientReq = new HttpRequestMessage();
                httpReq.CopyTo(httpClientReq);

                var httpClientResponse = await httpClient.SendAsync(httpClientReq);
                var httpResp = new SolidHttpResponse();
                await httpResp.CopyFromAsync(httpClientResponse);

                return MethodInfo.ExtractResponse<TAdvice>(httpResp);
            }
        }
    }
}
