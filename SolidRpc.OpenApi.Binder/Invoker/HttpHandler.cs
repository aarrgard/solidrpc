using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(HttpHandler), typeof(HttpHandler))]
namespace SolidRpc.OpenApi.Binder.Invoker
{
    public class HttpHandler : Handler
    {
        public HttpHandler(ILogger<HttpHandler> logger, IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory)
            :base(logger, serviceProvider)
        {
            Logger = logger;
            HttpClientFactory = httpClientFactory;
        }

        public ILogger Logger { get; }
        public IHttpClientFactory HttpClientFactory { get; }

        public override ITransport GetTransport(IEnumerable<ITransport> transports)
        {
            return transports.OfType<IHttpTransport>().FirstOrDefault();
        }

        protected override async Task<IHttpResponse> InvokeAsync<TResp>(IMethodBinding methodBinding, ITransport transport, IHttpRequest httpReq, CancellationToken cancellationToken)
        {
            var httpTransport = (IHttpTransport)transport;
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
