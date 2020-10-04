using Microsoft.IdentityModel.Tokens;
using SolidRpc.Abstractions.Types.OAuth2;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.OAuth2.InternalServices
{
    /// <summary>
    /// Represents an authority
    /// </summary>
    public interface IAuthority
    {
        /// <summary>
        /// Returns the discovery document
        /// </summary>
        Task<OpenIDConnnectDiscovery> GetDiscoveryDocumentAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the principal for supplied jwt token
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPrincipal> GetPrincipalAsync(string jwt, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the signing keys
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<SecurityKey>> GetSigningKeysAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the client jwt from the authority
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetClientJwtAsync(string clientId, string clientSecret, CancellationToken cancellationToken = default(CancellationToken));
    }
}