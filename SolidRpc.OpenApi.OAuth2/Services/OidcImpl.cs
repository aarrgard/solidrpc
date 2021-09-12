using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.Abstractions.Types.OAuth2;
using SolidRpc.OpenApi.OAuth2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

[assembly: SolidRpcService(typeof(ISolidRpcOidc), typeof(SolidRpcOidcImpl), SolidRpcServiceLifetime.Scoped)]
namespace SolidRpc.OpenApi.OAuth2.Services
{
    public class SolidRpcOidcImpl : ISolidRpcOidc
    {
        public SolidRpcOidcImpl(
            IInvoker<ISolidRpcOidc> invoker,
            IAuthorityLocal localAuthority)
        {
            LocalAuthority = localAuthority;
            Invoker = invoker;
        }

        private IAuthorityLocal LocalAuthority { get; }
        private IInvoker<ISolidRpcOidc> Invoker { get; }

        public async Task<OpenIDConnectDiscovery> GetDiscoveryDocumentAsync(CancellationToken cancellationToken = default)
        {
            var authorizationEndpoint = await Invoker.GetUriAsync(o => o.AuthorizeAsync(null, null, null, null, null, cancellationToken), false);
            var tokenEndpoint = await Invoker.GetUriAsync(o => o.GetTokenAsync(null, null, null, null, null, null, null, null, null, null, cancellationToken), false);
            var jwksUri = await Invoker.GetUriAsync(o => o.GetKeysAsync(cancellationToken), false);
            return new OpenIDConnectDiscovery()
            {
                Issuer = LocalAuthority.Authority,
                AuthorizationEndpoint = authorizationEndpoint,
                TokenEndpoint = tokenEndpoint,
                JwksUri = jwksUri,
            };
        }

        public async Task<OpenIDKeys> GetKeysAsync(CancellationToken cancellationToken = default)
        {
            var keys = await LocalAuthority.GetSigningKeysAsync(cancellationToken);
            return new OpenIDKeys() { Keys = keys.ToArray() };
        }

        public async Task<FileContent> AuthorizeAsync(IEnumerable<string> scope, string responseType, string clientId, string redirectUri = null, string state = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(redirectUri)) throw new ArgumentNullException(nameof(redirectUri));
            if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrEmpty(responseType)) throw new ArgumentNullException(nameof(responseType));
            var ci = new ClaimsIdentity(new[] { new Claim("AllowedPath", "/*") });
            var idToken = await LocalAuthority.CreateAccessTokenAsync(ci, null, cancellationToken);
            var session_state = $"&session_state={Guid.NewGuid()}";
            state = string.IsNullOrEmpty(state) ? "" : $"&state={HttpUtility.UrlEncode(state)}";
            return new FileContent()
            {
                Location = $"{redirectUri}?{responseType}={idToken.AccessToken}{session_state}{state}"
            };
        }

        public Task<TokenResponse> GetTokenAsync(string grantType = null, string clientId = null, string clientSecret = null, string username = null, string password = null, IEnumerable<string> scope = null, string code = null, string redirectUri = null, string codeVerifier = null, string refreshToken = null, CancellationToken cancellationToken = default)
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
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new  Claim("client_id", clientId));
            var accessToken = await LocalAuthority.CreateAccessTokenAsync(claimsIdentity, null, cancellationToken);
            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
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
                claimsIdentity.AddClaim(new  Claim("scope", scope));
            }
            var accessToken = await LocalAuthority.CreateAccessTokenAsync(claimsIdentity, null, cancellationToken);
            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
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
                ExpiresIn = accessToken.ExpiresIn,
                Scope = string.Join(" ", scopes),
                TokenType = accessToken.TokenType
            };
        }
        private async Task<TokenResponse> GetTokenAsyncAuthorizationCode(string clientId, string clientSecret, string code, string redirectUri, string codeVerifier, CancellationToken cancellationToken)
        {
            var prin = await LocalAuthority.GetPrincipalAsync(code, null, cancellationToken);
            var accessToken = await LocalAuthority.CreateAccessTokenAsync((ClaimsIdentity)prin.Identity, null, cancellationToken);
            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
                ExpiresIn = accessToken.ExpiresIn,
                TokenType = accessToken.TokenType
            };
        }
    }
}
