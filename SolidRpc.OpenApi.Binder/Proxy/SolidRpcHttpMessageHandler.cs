using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.OpenApi.Binder.Http;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    /// <summary>
    /// Message handler that can be registered in the IoC container to invoke services
    /// in an other IoC container.
    /// </summary>
    public class SolidRpcHttpMessageHandler : HttpMessageHandler
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="methodInvoker">The method invoker.</param>
        public SolidRpcHttpMessageHandler(IMethodInvoker methodInvoker)
        {
            MethodInvoker = methodInvoker;
        }

        /// <summary>
        /// The method invoker
        /// </summary>
        public IMethodInvoker MethodInvoker { get; }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var req = new SolidHttpRequest();
            await req.CopyFromAsync(request);
            var resp = await MethodInvoker.InvokeAsync(req, cancellationToken);
            var response = new HttpResponseMessage();
            await resp.CopyToAsync(response, request);
            return response;
        }
    }
}
