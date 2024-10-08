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
        [Security(nameof(SolidRpcHostPermission))]
        Task<Guid> GetHostId(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the id of this host. This method can be used to determine if a host is up and running by
        /// comparing the returned value with the instance that we want to send to. If a host goes down it is 
        /// removed from the router and another instance probably responds to the call.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Security(nameof(SolidRpcHostPermission))]
        Task<SolidRpcHostInstance> GetHostInstance(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// This method is invoked on all the hosts in a store when a new host is available.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Security(nameof(SolidRpcHostPermission))]
        Task<IEnumerable<SolidRpcHostInstance>> SyncHostsFromStore(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Invokes the "GetHostInstance" targeted for supplied instance and resturns the result
        /// </summary>
        /// <param name="hostInstance"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Security(nameof(SolidRpcHostPermission))]
        Task<SolidRpcHostInstance> CheckHost(SolidRpcHostInstance hostInstance, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the host configuration.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Security(nameof(SolidRpcHostPermission))]
        Task<IEnumerable<NameValuePair>> GetHostConfiguration(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Function that determines if the host is alive.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Security(nameof(SolidRpcHostPermission))]
        Task IsAlive(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the base url for this host
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Security(nameof(SolidRpcHostPermission))]
        Task<Uri> BaseAddress(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the base url for this host
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Security(nameof(SolidRpcHostPermission))]
        Task<IEnumerable<string>> AllowedCorsOrigins(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the default timezone
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Security(nameof(SolidRpcHostPermission))]
        Task<string> DefaultTimezone(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the list of allowed cors origins.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Security(nameof(SolidRpcHostPermission))]
        Task<DateTimeOffset> ParseDateTime(string dateTime, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all the assembly names that are located on this host
        /// </summary>
        /// <returns></returns>
        [Security(nameof(SolidRpcHostPermission))]
        Task<IEnumerable<string>> ListAssemblyNames(CancellationToken cancellationToken = default);
    }
}
