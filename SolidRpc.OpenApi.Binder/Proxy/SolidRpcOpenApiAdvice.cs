using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.OpenApi.Binder.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SolidRpcOpenApiAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        /// <summary>
        /// Constucts a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="openApiParser"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="serviceProvider"></param>
        public SolidRpcOpenApiAdvice(
            ILogger<SolidRpcOpenApiAdvice<TObject, TMethod, TAdvice>> logger,
            IMethodBinderStore methodBinderStore,
            IHttpClientFactory httpClientFactory)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            MethodBinderStore = methodBinderStore ?? throw new ArgumentNullException(nameof(methodBinderStore));
            HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        private HttpMessageHandler HttpMessageHandler { get; }
        private ILogger Logger { get; }
        private IOpenApiParser OpenApiParser { get; }
        private IMethodBinderStore MethodBinderStore { get; }
        public IHttpClientFactory HttpClientFactory { get; }
        private IMethodInfo MethodInfo { get; set; }

        /// <summary>
        /// Confugures the proxy
        /// </summary>
        /// <param name="config"></param>
        public void Configure(ISolidRpcOpenApiConfig config)
        {
            MethodInfo = MethodBinderStore.GetMethodInfo(
                config.GetOpenApiConfiguration(),
                config.InvocationConfiguration.MethodInfo,
                config.BaseUriTransformer
            );
        }

        /// <summary>
        /// Handles  the invocation
        /// </summary>
        /// <param name="next"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public async Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            var httpClientName = MethodInfo.MethodBinder.OpenApiSpec.Title;
            if(Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Getting http client for '{httpClientName}'");
            }
            var httpClient = HttpClientFactory.CreateClient(httpClientName);
            var httpReq = new SolidHttpRequest();
            await MethodInfo.BindArgumentsAsync(httpReq, invocation.Arguments);

            if (HttpMessageHandler == null)
            {
                Logger.LogTrace($"Sending data to remote host {httpReq.Scheme}://{httpReq.HostAndPort}{httpReq.Path}");
            }
            else
            {
                Logger.LogTrace($"Sending data to local handler. Emulating host {httpReq.Scheme}://{httpReq.HostAndPort}{httpReq.Path}");
            }

            var httpClientReq = new HttpRequestMessage();
            httpReq.CopyTo(httpClientReq);

            var httpClientResponse = await httpClient.SendAsync(httpClientReq);
            var httpResp = new SolidHttpResponse();
            await httpResp.CopyFromAsync(httpClientResponse);

            return MethodInfo.ExtractResponse<TAdvice>(httpResp);
        }
    }
}
