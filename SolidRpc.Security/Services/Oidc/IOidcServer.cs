using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Security.Types;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SolidRpc.Security.Services.Oidc {
    /// <summary>
    /// Defines logic for the oidc server
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IOidcServer {
        /// <summary>
        /// Returns the /.well-known/openid-configuration file
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<OpenIDConnnectDiscovery> OAuth2Discovery(
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Returns the keys
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<OpenIDKeys> OAuth2Keys(
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// authenticates a user
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<TokenResponse> OAuth2TokenGet(
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
        Task<TokenResponse> OAuth2TokenPost(
            string grantType = default(string),
            string clientId = default(string),
            string clientSecret = default(string),
            string username = default(string),
            string password = default(string),
            IEnumerable<string> scope = default(IEnumerable<string>),
            string code = default(string),
            string redirectUri = default(string),
            string codeVerifier = default(string),
            string refreshToken = default(string),
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
        Task<WebContent> OAuth2AuthorizeGet(
            IEnumerable<string> scope,
            string responseType,
            string clientId,
            string redirectUri = default(string),
            string state = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// authorizes a user
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<WebContent> OAuth2AuthorizePost(
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}