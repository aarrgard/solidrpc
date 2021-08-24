using SolidRpc.Tests.Swagger.SpecGen.OAuth2.Types;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.SpecGen.OAuth2.Services
{
    /// <summary>
    /// Creates an oauth2 spec
    /// </summary>
    public interface IOAuth2
    {
        /// <summary>
        /// Returns the discovery document
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [OpenApi(Path = "/.well-known/openid-configuration")]
        Task<OpenIDConnectDiscovery> Discovery(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grantType"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="scope"></param>
        /// <param name="code"></param>
        /// <param name="redirectUri"></param>
        /// <param name="codeVerifier"></param>
        /// <param name="refreshToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [OpenApi(Name = "token", Verbs = new[] { "Post", "Get" })]
        Task<TokenResponse> Token(
             [OpenApi(Name = "grant_type")] string grantType = default(string),
             [OpenApi(Name = "client_id")] string clientId = default(string),
             [OpenApi(Name = "client_secret")] string clientSecret = default(string),
             [OpenApi(Name = "username")] string username = default(string),
             [OpenApi(Name = "password")] string password = default(string),
             [OpenApi(Name = "scope")] IEnumerable<string> scope = default(IEnumerable<string>),
             [OpenApi(Name = "code")] string code = default(string),
             [OpenApi(Name = "redirect_uri")] string redirectUri = default(string),
             [OpenApi(Name = "code_verifier")] string codeVerifier = default(string),
             [OpenApi(Name = "refresh_token")] string refreshToken = default(string),
             CancellationToken cancellationToken = default(CancellationToken));

    }
}
