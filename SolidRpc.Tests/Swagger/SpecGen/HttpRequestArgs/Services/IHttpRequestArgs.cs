using SolidRpc.Tests.Swagger.SpecGen.HttpRequestArgs.Types;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.SpecGen.HttpRequestArgs.Services
{
    /// <summary>
    /// Invokes the request.
    /// </summary>
    public interface IHttpRequestArgs
    {
        /// <summary>
        /// Invokes the request
        /// </summary>
        /// <param name="req"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<HttpRequest> TestInvokeRequest(HttpRequest req, CancellationToken cancellationToken = default(CancellationToken));
    }
}
