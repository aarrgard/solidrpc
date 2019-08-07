using SolidRpc.Security.Security;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Services
{
    /// <summary>
    /// This service handles the 
    /// </summary>
    public interface ISolidRpcSecurity
    {
        /// <summary>
        /// Returns the values(permissions) that the authenticated user or client is granted.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Security(nameof(SolidRpcSecurityPermission))]
        Task<IEnumerable<string>> IsAllowedAccess(
            string attribute, 
            string key, 
            IEnumerable<string> values, 
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
