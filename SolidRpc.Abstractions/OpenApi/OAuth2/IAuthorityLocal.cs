using SolidRpc.Abstractions.Types.OAuth2;
using System;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.OAuth2
{
    /// <summary>
    /// Represents additional functionality for a local
    /// authority.
    /// </summary>
    public interface IAuthorityLocal : IAuthority
    {
        /// <summary>
        /// Creates a new set of signing keys for this authority.
        /// </summary>
        void CreateSigningKey();

        /// <summary>
        /// Sets the signing keys
        /// </summary>
        /// <param name="cert"></param>
        void SetSigningKey(X509Certificate2 cert);

        /// <summary>
        /// The signing key
        /// </summary>
        OpenIDKey PrivateSigningKey { get; }

        /// <summary>
        /// Creates the jwt token from supplied claims
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <param name="expiryTime"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TokenResponse> CreateAccessTokenAsync(
            ClaimsIdentity claimsIdentity, 
            TimeSpan? expiryTime = null, 
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
