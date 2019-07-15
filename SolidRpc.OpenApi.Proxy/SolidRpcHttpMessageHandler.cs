using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Proxy;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Proxy
{
    /// <summary>
    /// Message handler that can be registered in the IoC container to invoke services
    /// in an othe IoC container.
    /// </summary>
    public class SolidRpcHttpMessageHandler : HttpMessageHandler
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="serviceProvider">The service provider for the services.</param>
        public SolidRpcHttpMessageHandler(IMethodInvoker methodInvoker)
        {
            MethodInvoker = methodInvoker;
        }

        public IMethodInvoker MethodInvoker { get; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var req = new HttpRequest();
            await req.CopyFrom(request);
            var resp = await MethodInvoker.InvokeAsync(req, cancellationToken);
            var response = new HttpResponseMessage();
            await resp.CopyTo(response);
            return response;
        }
    }
}
