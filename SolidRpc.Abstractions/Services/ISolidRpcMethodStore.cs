using SolidRpc.Abstractions.Types;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Implements the logic to get and store invocations for a user/session
    /// </summary>
    public interface ISolidRpcMethodStore
    {
        /// <summary>
        /// Returns the http requests for the current user.
        /// </summary>
        /// <param name="takeCount"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IEnumerable<HttpRequest>> GetHttpRequestAsync(
            int? takeCount = 1,
            CancellationToken cancellation = default(CancellationToken));

        /// <summary>
        /// Returns the http request for the current user.
        /// </summary>
        /// <param name="solidRpcCallId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task RemoveHttpRequestAsync(
            string solidRpcCallId,
            CancellationToken cancellation = default(CancellationToken));

        /// <summary>
        /// Returns the next http requests for supplied session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="takeCount"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IEnumerable<HttpRequest>> GetHttpRequestsAsync(
            string sessionId,
            int? takeCount = 1,
            CancellationToken cancellation = default(CancellationToken));

        /// <summary>
        /// Removes the http request 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="solidRpcCallId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task RemoveHttpRequestAsync(
            string sessionId,
            string solidRpcCallId,
            CancellationToken cancellation = default(CancellationToken));
    }
}
