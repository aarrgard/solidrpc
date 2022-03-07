using SolidRpc.Abstractions.Types.OAuth2;
using System;
using System.Collections.Generic;
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
        /// <param name="removeOld">specifies if the old key should be removed or kept</param>
        void CreateSigningKey(bool removeOld = false);

        /// <summary>
        /// Sets the signing key that is to be used for signing.
        /// </summary>
        /// <param name="cert"></param>
        /// <param name="keyId"></param>
        void SetSigningKey(X509Certificate2 cert, Func<X509Certificate2, string> keyId = null);

        /// <summary>
        /// Adds a signing key to the set of keys.
        /// </summary>
        /// <param name="cert"></param>
        /// <param name="keyId"></param>
        void AddSigningKey(X509Certificate2 cert, Func<X509Certificate2, string> keyId = null);

        /// <summary>
        /// The signing key
        /// </summary>
        OpenIDKey PrivateSigningKey { get; }

        /// <summary>
        /// The signing key
        /// </summary>
        OpenIDKey PublicSigningKey { get; }

        /// <summary>
        /// Encrypts using the supplied key.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] Encrypt(byte[] data);

        /// <summary>
        /// Encrypts using the supplied key.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] Decrypt(byte[] data);

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
