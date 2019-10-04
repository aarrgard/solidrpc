using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace SolidRpc.Security.Front.InternalServices
{
    /// <summary>
    /// This interface defines logic to interact with the jwt token factory.
    /// </summary>
    public interface IAccessTokenFactory
    {
        /// <summary>
        /// Returns the issuer for the tokens
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetIssuerAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the public signing keys.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<RsaSecurityKey>> GetSigningPublicKeys();

        /// <summary>
        /// Constructs a new Jwt token with all the claims in supplied identity.
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <returns></returns>
        Task<IAccessToken> CreateAccessToken(ClaimsIdentity claimsIdentity, CancellationToken cancellationToken = default(CancellationToken));
    }
}
