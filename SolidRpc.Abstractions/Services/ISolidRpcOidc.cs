using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Types;
using SolidRpc.Abstractions.Types.OAuth2;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Implements logic for the oidc server
    /// </summary>
    public interface ISolidRpcOidc
    {
        /// <summary>
        /// Returns the /.well-known/openid-configuration file
        /// </summary>
        /// <param name="cancellationToken"></param>
        [OpenApi(Path = "/.well-known/openid-configuration")]
        Task<OpenIDConnectDiscovery> GetDiscoveryDocumentAsync(
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the keys
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<OpenIDKeys> GetKeysAsync(
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// authenticates a user
        /// </summary>
        /// <param name="grantType">The the grant type</param>
        /// <param name="clientId">The the client id</param>
        /// <param name="clientSecret">The client secret</param>
        /// <param name="username">The user name</param>
        /// <param name="password">The the user password</param>
        /// <param name="scope">The the scopes</param>
        /// <param name="code">The the code</param>
        /// <param name="redirectUri">The the redirect uri</param>
        /// <param name="codeVerifier">The the code verifier</param>
        /// <param name="refreshToken">The the refresh token</param>
        /// <param name="cancellationToken"></param>
        Task<TokenResponse> GetTokenAsync(
            [OpenApi(Name = "grant_type", In = "formData")] string grantType = default(string),
            [OpenApi(Name = "client_id", In = "formData")] string clientId = default(string),
            [OpenApi(Name = "client_secret", In = "formData")] string clientSecret = default(string),
            [OpenApi(Name = "username", In = "formData")] string username = default(string),
            [OpenApi(Name = "password", In = "formData")] string password = default(string),
            [OpenApi(Name = "scope", In = "formData", CollectionFormat="ssv")] IEnumerable<string> scope = default(IEnumerable<string>),
            [OpenApi(Name = "code", In = "formData")] string code = default(string),
            [OpenApi(Name = "redirect_uri", In = "formData")] string redirectUri = default(string),
            [OpenApi(Name = "code_verifier", In = "formData")] string codeVerifier = default(string),
            [OpenApi(Name = "refresh_token", In = "formData")] string refreshToken = default(string),
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// authorizes a user
        /// </summary>
        /// <param name="scope">REQUIRED. OpenID Connect requests MUST contain the openid scope value. If the openid scope value is not present, the behavior is entirely unspecified. Other scope values MAY be present. Scope values used that are not understood by an implementation SHOULD be ignored. See Sections 5.4 and 11 for additional scope values defined by this specification.</param>
        /// <param name="responseType">REQUIRED. OAuth 2.0 Response Type value that determines the authorization processing flow to be used, including what parameters are returned from the endpoints used. When using the Authorization Code Flow, this value is code.</param>
        /// <param name="clientId">REQUIRED. OAuth 2.0 Client Identifier valid at the Authorization Server.</param>
        /// <param name="redirectUri">REQUIRED. Redirection URI to which the response will be sent. This URI MUST exactly match one of the Redirection URI values for the Client pre-registered at the OpenID Provider, with the matching performed as described in Section 6.2.1 of [RFC3986] (Simple String Comparison). When using this flow, the Redirection URI SHOULD use the https scheme; however, it MAY use the http scheme, provided that the Client Type is confidential, as defined in Section 2.1 of OAuth 2.0, and provided the OP allows the use of http Redirection URIs in this case. The Redirection URI MAY use an alternate scheme, such as one that is intended to identify a callback into a native application.</param>
        /// <param name="state">RECOMMENDED. Opaque value used to maintain state between the request and the callback. Typically, Cross-Site Request Forgery (CSRF, XSRF) mitigation is done by cryptographically binding the value of this parameter with a browser cookie.</param>
        /// <param name="cancellationToken"></param>
        Task<FileContent> AuthorizeAsync(
            [OpenApi(Name = "scope", In = "query")] IEnumerable<string> scope,
            [OpenApi(Name = "response_type", In = "query")] string responseType,
            [OpenApi(Name = "client_id", In = "query")] string clientId,
            [OpenApi(Name = "redirect_uri", In = "query")] string redirectUri = default(string),
            [OpenApi(Name = "state", In = "query")] string state = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
