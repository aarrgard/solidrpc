using SolidRpc.Abstractions.Types.OAuth2;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.OAuth2
{
    /// <summary>
    /// Represents an authority
    /// </summary>
    public interface IAuthority
    {
        /// <summary>
        /// The uri of the issuer
        /// </summary>
        Uri Authority { get; }

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
        Task<IEnumerable<OpenIDKey>> GetSigningKeysAsync(CancellationToken cancellationToken = default(CancellationToken));

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