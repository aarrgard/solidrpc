using SolidRpc.Abstractions.Types;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// The host store is responsible for storing persistent information about 
    /// a host in a cluster. Usually hosts are placed behind a load balancer that
    /// can route to a specific host based on some cookie or header information.
    /// </summary>
    public interface ISolidRpcHostStore
    {
        /// <summary>
        /// Adds a host instance to the host store.
        /// </summary>
        /// <param name="hostInstance"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddHostInstanceAsync(SolidRpcHostInstance hostInstance, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Removes a host instance from the store.
        /// </summary>
        /// <param name="hostInstance"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoveHostInstanceAsync(SolidRpcHostInstance hostInstance, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Lists the stored host instances
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<SolidRpcHostInstance>> ListHostInstancesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
