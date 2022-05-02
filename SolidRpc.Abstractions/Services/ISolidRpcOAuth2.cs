using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Interfaces that defines the logic for OAuth2 support.
    /// </summary>
    public interface ISolidRpcOAuth2
    {
        /// <summary>
        /// This method returns a html page that authorizes the user. When the user
        /// has been authorized the supplied callback is invoked with the "access_token"
        /// supplied as a query parameter along with the "state".
        /// 
        /// Use this method to retreive tokens from a standalone node instance or from a SPA(single page app)
        /// 
        /// Start a local http server and supply the address to the handler.
        /// </summary>
        /// <param name="callbackUri">the callback</param>
        /// <param name="state">the state</param>
        /// <param name="scopes">the scopes</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> GetAuthorizationCodeTokenAsync(
            Uri callbackUri = null,
            string state = null,
            IEnumerable<string> scopes = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// This is the method that is invoked when a user has been authenticated
        /// and a valid token is supplied. The authentication uses the "authorization code" flow
        /// so this method retreives the access token using supplied code.
        /// </summary>
        /// <param name="code">code token to use</param>
        /// <param name="state">the state</param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<FileContent> TokenCallbackAsync(
            string code = null,
            string state = null,
            CancellationToken cancellation = default);

        /// <summary>
        /// Use this method to refresh a token obtained from the callback.
        /// 
        /// This method fetches a new token from the OAuth server using the refresh token stored as a cookie when authorizing for the first time.
        /// </summary>
        /// <param name="accessToken">the token to refresh - may be an expired token</param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<FileContent> RefreshTokenAsync(
            string accessToken,
            CancellationToken cancellation = default);

        /// <summary>
        /// Performs the logout @ the identity server.
        /// </summary>
        /// <param name="callbackUri"></param>
        /// <param name="accessToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> LogoutAsync(
            Uri callbackUri = null,
            string accessToken = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs the logout @ the identity server.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileContent> PostLogoutAsync(
            string state = null,
            CancellationToken cancellationToken = default);
    }
}
