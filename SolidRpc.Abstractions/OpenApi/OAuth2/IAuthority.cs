using SolidRpc.Abstractions.Types.OAuth2;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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
        /// Returns the authority factory
        /// </summary>
        IAuthorityFactory AuthorityFactory { get; }

        /// <summary>
        /// The uri of the issuer
        /// </summary>
        string Authority { get; }

        /// <summary>
        /// Adds a set of default scopes to request based on the supplied grant type.
        /// </summary>
        /// <param name="grantType"></param>
        /// <param name="scopes"></param>
        void AddDefaultScopes(string grantType, IEnumerable<string> scopes);

        /// <summary>
        /// Returns the scopes for supplied grant type
        /// </summary>
        /// <param name="grantType"></param>
        /// <param name="additionalScopes"></param>
        /// <returns></returns>
        IEnumerable<string> GetScopes(string grantType, IEnumerable<string> additionalScopes);

        /// <summary>
        /// Returns the discovery document
        /// </summary>
        Task<OpenIDConnectDiscovery> GetDiscoveryDocumentAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the principal for supplied jwt token
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="tokenChecks"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ClaimsPrincipal> GetPrincipalAsync(string jwt, Action<IAuthorityTokenChecks> tokenChecks = null, CancellationToken cancellationToken = default(CancellationToken));

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
        /// <param name="scopes"></param>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TokenResponse> GetClientJwtAsync(
            string clientId, 
            string clientSecret, 
            IEnumerable<string> scopes, 
            TimeSpan? timeout = null, 
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the client jwt from the authority
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="scopes"></param>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TokenResponse> GetUserJwtAsync(
            string clientId, 
            string clientSecret, 
            string userId, 
            string password, 
            IEnumerable<string> scopes,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns a token from supplied code.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="code"></param>
        /// <param name="timeout"></param>
        /// <param name="redirectUri"></param>
        /// <param name="cancellationToken"></param>
        Task<TokenResponse> GetCodeJwtToken(
            string clientId,
            string clientSecret,
            string code,
            string redirectUri,
            TimeSpan? timeout = null, 
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Refreshes a token
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="refreshToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TokenResponse> RefreshTokenAsync(
            string clientId,
            string clientSecret,
            string refreshToken,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Revokes the supplied token
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="token"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RevokeTokenAsync(
            string clientId,
            string clientSecret,
            string token,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Verifies the supplied data using the signature provided
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> VerifyData(
            byte[] data, 
            byte[] signature,
            CancellationToken cancellationToken = default);
    }
}