using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(HttpHandler), typeof(HttpHandler))]
[assembly: SolidRpcService(typeof(IHandler), typeof(HttpHandler), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.OpenApi.Binder.Invoker
{
    /// <summary>
    /// Represents the HttpHandler
    /// </summary>
    public class HttpHandler : Handler
    {
        /// <summary>
        /// Returns the "Http" transport type
        /// </summary>
        public static new string TransportType => GetTransportType(typeof(HttpHandler));
            
        public HttpHandler(ILogger<HttpHandler> logger, IServiceProvider serviceProvider)
            :base(logger, serviceProvider)
        {
            Logger = logger;
        }

        public ILogger Logger { get; }
        public IHttpClientFactory HttpClientFactory => ServiceProvider.GetRequiredService<IHttpClientFactory>();

        public override async Task<IHttpResponse> InvokeAsync<TResp>(IMethodBinding methodBinding, ITransport transport, IHttpRequest httpReq, InvocationOptions invocationOptions, CancellationToken cancellationToken)
        {
            var httpClientName = methodBinding.MethodBinder.OpenApiSpec.Title;
            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.LogTrace($"Getting http client for '{httpClientName}'");
            }
            var httpClient = HttpClientFactory.CreateClient(httpClientName);

            var httpClientReq = new HttpRequestMessage();
            httpReq.CopyTo(httpClientReq);

            var httpClientResponse = await httpClient.SendAsync(httpClientReq);
            var httpResp = new SolidHttpResponse();
            await httpResp.CopyFromAsync(httpClientResponse);

            return httpResp;
        }
    }
}
