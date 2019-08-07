using SolidRpc.Abstractions.Security;
using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Represents a solid rpc host.
    /// </summary>
    public interface ISolidRpcHost
    {
        /// <summary>
        /// Returns the id of this host
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Security(Public = true)]
        Task<Guid> GetHostId(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the host configuration.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Security(nameof(SolidRpcAdminPermission))]
        Task<IEnumerable<NameValuePair>> GetHostConfiguration(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Function that determines if the host is alive.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Security(nameof(SolidRpcAdminPermission))]
        Task IsAlive(CancellationToken cancellationToken = default(CancellationToken));
    }
}
