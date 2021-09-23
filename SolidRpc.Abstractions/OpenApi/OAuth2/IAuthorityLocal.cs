using SolidRpc.Abstractions.Types.OAuth2;
using System;
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
        /// <param name="keyId"></param>
        void SetSigningKey(X509Certificate2 cert, Func<X509Certificate2, string> keyId = null);

        /// <summary>
        /// The signing key
        /// </summary>
        OpenIDKey PrivateSigningKey { get; }

        /// <summary>
        /// Creates the jwt token from supplied claims
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <param name="expires"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TokenResponse> CreateAccessTokenAsync(
            ClaimsIdentity claimsIdentity, 
            DateTimeOffset? expires = null, 
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
