using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AspNetCore.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(ISolidRpcHostStore), typeof(SolidRpcHostStoreMemory), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.OpenApi.AspNetCore.Services
{
    /// <summary>
    /// An in memory host store
    /// </summary>
    public class SolidRpcHostStoreMemory : ISolidRpcHostStore
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public SolidRpcHostStoreMemory()
        {
            KnownHosts = new ConcurrentDictionary<Guid, SolidRpcHostInstance>();
        }
        private ConcurrentDictionary<Guid, SolidRpcHostInstance> KnownHosts { get; }

        /// <summary>
        /// Adds a host to the store.
        /// </summary>
        /// <param name="hostInstance"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task AddHostInstanceAsync(SolidRpcHostInstance hostInstance, CancellationToken cancellationToken = default(CancellationToken))
        {
            KnownHosts.GetOrAdd(hostInstance.HostId, hostInstance);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Lists all the hosts
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IEnumerable<SolidRpcHostInstance>> ListHostInstancesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(KnownHosts.Values.AsEnumerable());
        }

        /// <summary>
        /// Removes the host
        /// </summary>
        /// <param name="hostInstance"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task RemoveHostInstanceAsync(SolidRpcHostInstance hostInstance, CancellationToken cancellationToken = default(CancellationToken))
        {
            KnownHosts.TryRemove(hostInstance.HostId, out SolidRpcHostInstance dummy);
            return Task.CompletedTask;
        }
    }
}
