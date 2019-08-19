using System.Threading.Tasks;
using SolidRpc.Security.Types;
using System.Threading;
using System.Collections.Generic;
using System;
namespace SolidRpc.Security.Services {
    /// <summary>
    /// Defines logic for solid rpc security
    /// </summary>
    public interface ISolidRpcSecurity {
        /// <summary>
        /// Returns the login page
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<WebContent> LoginPage(
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Returns the script paths to use for logging in.
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Uri>> LoginScripts(
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Returns the status at each login provider
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<LoginProvider>> LoginProviders(
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}