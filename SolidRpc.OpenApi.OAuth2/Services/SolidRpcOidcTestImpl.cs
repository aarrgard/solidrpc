using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.Abstractions.Types.OAuth2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SolidRpc.OpenApi.OAuth2.Services
{
    /// <summary>
    /// Implements an Oidc server
    /// </summary>
    public class SolidRpcOidcTestImpl : SolidRpcOidcImpl
    {
        private static IDictionary<string, ClaimsIdentity> RefreshTokens = new Dictionary<string, ClaimsIdentity>();

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="localAuthority"></param>
        public SolidRpcOidcTestImpl(
            IInvoker<ISolidRpcOidc> invoker,
            IAuthorityLocal localAuthority)
            :base(invoker, localAuthority)
        {
        }

        private string CreateRefreshToken(ClaimsIdentity claimsIdentity)
        {
            var refreshToken = Guid.NewGuid().ToString();
            RefreshTokens[refreshToken] = claimsIdentity;
            return refreshToken;
        }

        /// <summary>
        /// Returns the discovery document
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<OpenIDConnectDiscovery> GetDiscoveryDocumentAsync(CancellationToken cancellationToken = default)
        {
            var authorizationEndpoint = await Invoker.GetUriAsync(o => o.AuthorizeAsync(null, null, null, null, null, null, null, cancellationToken), false);
            var revokationEndpoint = await Invoker.GetUriAsync(o => o.RevokeAsync(null, null, null, null, cancellationToken), false);
            var tokenEndpoint = await Invoker.GetUriAsync(o => o.GetTokenAsync(null, null, null, null, null, null, null, null, null, null, cancellationToken), false);
            var jwksUri = await Invoker.GetUriAsync(o => o.GetKeysAsync(cancellationToken), false);
            return new OpenIDConnectDiscovery()
            {
                Issuer = LocalAuthority.Authority,
                AuthorizationEndpoint = authorizationEndpoint,
                TokenEndpoint = tokenEndpoint,
                RevocationEndpoint = revokationEndpoint,
                JwksUri = jwksUri,
            };
        }

        /// <summary>
        /// Get keys
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<OpenIDKeys> GetKeysAsync(CancellationToken cancellationToken = default)
        {
            var keys = await LocalAuthority.GetSigningKeysAsync(cancellationToken);
            return new OpenIDKeys() { Keys = keys.ToArray() };
        }

        /// <summary>
        /// Authorizes a client or user
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="responseType"></param>
        /// <param name="clientId"></param>
        /// <param name="redirectUri"></param>
        /// <param name="state"></param>
        /// <param name="responseMode"></param>
        /// <param name="nonce"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<FileContent> AuthorizeAsync(
            IEnumerable<string> scope,
            string responseType, 
            string clientId, 
            string redirectUri = null, 
            string state = null, 
            string responseMode = null, 
            string nonce = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(redirectUri)) throw new ArgumentNullException(nameof(redirectUri));
            if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrEmpty(responseType)) throw new ArgumentNullException(nameof(responseType));

            var claims = new List<Claim>();
            claims.Add(new Claim("iss", LocalAuthority.Authority));
            claims.Add(new Claim("aud", clientId));
            claims.Add(new Claim("sub", Guid.NewGuid().ToString()));
            claims.Add(new Claim("AllowedPath", "/*"));
            if(nonce != null)
            {
                claims.Add(new Claim("nonce", nonce));
            }
            var ci = new ClaimsIdentity(claims);
            var idToken = await LocalAuthority.CreateAccessTokenAsync(ci, null, cancellationToken);
            var session_state = $"&session_state={Guid.NewGuid()}";

            var accessToken = idToken.AccessToken;
            if (responseType == "code")
            {
                accessToken = CreateRefreshToken(ci);
            }

            if (string.Equals(responseMode, "form_post"))
            {
                var enc = Encoding.UTF8;
                return new FileContent()
                {
                    ContentType = $"text/html",
                    CharSet = enc.HeaderName,
                    Content = new MemoryStream(enc.GetBytes($@"<html>
<head>
    <title>Submit This Form</title>
</head>
    <body onload='javascript:document.forms[0].submit()'>
    <form method='post' action='{redirectUri}'>
      <input type='hidden' name='state' value='{state}' />
      <input type='hidden' name='{responseType}' value='{accessToken}'/>
    </form>
    </body>
</html>"))
                };
            }

            //
            // query
            //
            state = string.IsNullOrEmpty(state) ? "" : $"&state={HttpUtility.UrlEncode(state)}";
            switch (responseType)
            {
                case "id_token":
                case "code":
                    return new FileContent()
                    {
                        Location = $"{redirectUri}?{responseType}={accessToken}{session_state}{state}"
                    };
                default:
                    throw new Exception("Cannot handle response type:" + responseType);

            }
        }
        /// <summary>
        /// REturns the token
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
        public override Task<TokenResponse> GetTokenAsync(string grantType = null, string clientId = null, string clientSecret = null, string username = null, string password = null, IEnumerable<string> scope = null, string code = null, string redirectUri = null, string codeVerifier = null, string refreshToken = null, CancellationToken cancellationToken = default)
        {
            switch (grantType)
            {
                case "authorization_code":
                    return GetTokenAsyncAuthorizationCode(clientId, clientSecret, code, redirectUri, codeVerifier, cancellationToken);
                case "client_credentials":
                    return GetTokenAsyncClientCredentials(clientId, clientSecret, scope, cancellationToken);
                case "password":
                    return GetTokenAsyncPassword(clientId, clientSecret, username, password, scope, cancellationToken);
                case "refresh_token":
                    return GetTokenAsyncRefreshToken(clientId, clientSecret, refreshToken, cancellationToken);
                default:
                    throw new Exception("Invalid grant type:" + grantType);
            }
        }

        private async Task<TokenResponse> GetTokenAsyncRefreshToken(string clientId, string clientSecret, string refreshToken, CancellationToken cancellationToken)
        {
            if(!RefreshTokens.TryGetValue(refreshToken, out ClaimsIdentity claimsIdentity))
            {
                throw new Exception("Failed to find identity for refresh token:" + refreshToken);
            }
            var accessToken = await LocalAuthority.CreateAccessTokenAsync(claimsIdentity, null, cancellationToken);
            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
                RefreshToken = CreateRefreshToken(claimsIdentity),
                ExpiresIn = accessToken.ExpiresIn,
                TokenType = accessToken.TokenType
            };
        }

        private async Task<TokenResponse> GetTokenAsyncPassword(string clientId, string clientSecret, string username, string password, IEnumerable<string> scopes, CancellationToken cancellationToken)
        {
            var claimsIdentity = new  ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("client_id", clientId));
            claimsIdentity.AddClaim(new Claim("sub", username));
            claimsIdentity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, username));
            foreach (var scope in scopes)
            {
                claimsIdentity.AddClaim(new Claim("scope", scope));
            }
            var accessToken = await LocalAuthority.CreateAccessTokenAsync(claimsIdentity, null, cancellationToken);
            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
                RefreshToken = CreateRefreshToken(claimsIdentity),
                ExpiresIn = accessToken.ExpiresIn,
                Scope = string.Join(" ", scopes),
                TokenType = accessToken.TokenType
            };
        }

        private async Task<TokenResponse> GetTokenAsyncClientCredentials(string clientId, string clientSecret, IEnumerable<string> scopes, CancellationToken cancellationToken)
        {
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("client_id", clientId));
            claimsIdentity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, clientId));
            foreach (var scope in scopes)
            {
                claimsIdentity.AddClaim(new Claim("scope", scope));
            }
            var accessToken = await LocalAuthority.CreateAccessTokenAsync(claimsIdentity, null, cancellationToken);
            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
                RefreshToken = CreateRefreshToken(claimsIdentity),
                ExpiresIn = accessToken.ExpiresIn,
                Scope = string.Join(" ", scopes),
                TokenType = accessToken.TokenType
            };
        }
        private async Task<TokenResponse> GetTokenAsyncAuthorizationCode(string clientId, string clientSecret, string code, string redirectUri, string codeVerifier, CancellationToken cancellationToken)
        {
            var principal = RefreshTokens[code];
            if(principal == null)
            {
                throw new Exception("Cannot find identity for code:" + code);
            }
            var accessToken = await LocalAuthority.CreateAccessTokenAsync(principal, null, cancellationToken);
            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
                RefreshToken = CreateRefreshToken(principal),
                ExpiresIn = accessToken.ExpiresIn,
                Scope = string.Join(" ", principal.Claims.Where(o => o.Type == "scope").Select(o => o.Value)),
                TokenType = accessToken.TokenType,
            };
        }

        public override Task RevokeAsync(string clientId = null,  string clientSecret = null,  string token = null,  string tokenHint = null, CancellationToken cancellationToken = default)
        {
            RefreshTokens.Remove(token);
            return Task.CompletedTask;
        }
    }
}
