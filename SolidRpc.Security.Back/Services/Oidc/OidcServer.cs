﻿using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Security.Services.Oidc;
using SolidRpc.Security.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SolidRpc.Security.Back.Services
{
    public class OidcServer : IOidcServer
    {
        public OidcServer(
            IInvoker<IOidcServer> httpInvoker,
            IAuthorityLocal authorityLocal)
        {
            HttpInvoker = httpInvoker ?? throw new ArgumentNullException(nameof(httpInvoker));
            AuthorityLocal = authorityLocal ?? throw new ArgumentNullException(nameof(authorityLocal));
        }

        private IInvoker<IOidcServer> HttpInvoker { get; }
        private IAuthorityLocal AuthorityLocal { get; }

        public async Task<OpenIDConnnectDiscovery> OAuth2Discovery(CancellationToken cancellationToken = default(CancellationToken))
        {
            var doc = new OpenIDConnnectDiscovery();
            doc.Issuer = AuthorityLocal.Authority;
            doc.JwksUri = await HttpInvoker.GetUriAsync(o => o.OAuth2Keys(cancellationToken), false);
            doc.AuthorizationEndpoint = await HttpInvoker.GetUriAsync(o => o.OAuth2AuthorizeGet(null, null, null, null, null, cancellationToken), false);
            doc.TokenEndpoint = await HttpInvoker.GetUriAsync(o => o.OAuth2TokenGet(cancellationToken), false);
            return doc;
        }

        public async Task<OpenIDKeys> OAuth2Keys(CancellationToken cancellationToken = default(CancellationToken))
        {
            var keys = await AuthorityLocal.GetSigningKeysAsync();
            return new OpenIDKeys()
            {
                Keys = keys.Select(o => new OpenIDKey()
                {
                    Alg = o.Alg,
                    E = o.E,
                    Issuer = o.Issuer,
                    Kid = o.Kid,
                    Kty = o.Kty,
                    N = o.N,
                    Use = o.Use,
                    X5c = o.X5c,
                    X5t = o.X5t,
                    X5u = o.X5u
                }).ToArray()
            };
        }

        public Task<TokenResponse> OAuth2TokenGet(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TokenResponse> OAuth2TokenPost(string grantType = null, string clientId = null, string clientSecret = null, string username = null, string password = null, IEnumerable<string> scope = null, string code = null, string redirectUri = null, string codeVerifier = null, string refreshToken = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            switch (grantType)
            {
                case "client_credentials":
                    return OAuth2TokenClientCredentials(clientId, clientSecret, scope, cancellationToken);
                case "password":
                    return OAuth2TokenPassword(clientId, clientSecret, username, password, scope, cancellationToken);
                case "authorization_code":
                    return OAuth2TokenAuthorizationCode(clientId, clientSecret, code, redirectUri, codeVerifier, cancellationToken);
                case "refresh_token":
                    return OAuth2TokenRefreshToken(clientId, clientSecret, refreshToken, cancellationToken);
                default:
                    throw new Exception("Invalid grant type:"+grantType);
            }
        }

        private async Task<TokenResponse> OAuth2TokenRefreshToken(string clientId, string clientSecret, string refreshToken, CancellationToken cancellationToken)
        {
            var claimsIdentity = new System.Security.Claims.ClaimsIdentity();
            claimsIdentity.AddClaim(new System.Security.Claims.Claim("client_id", clientId));
            var accessToken = await AuthorityLocal.CreateAccessTokenAsync(claimsIdentity, null, cancellationToken);
            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
                ExpiresIn = accessToken.ExpiresIn,
                TokenType = accessToken.TokenType
            };
        }

        private async Task<TokenResponse> OAuth2TokenAuthorizationCode(string clientId, string clientSecret, string code, string redirectUri, string codeVerifier, CancellationToken cancellationToken)
        {
            ClaimsPrincipal prin;
            if (code == "my code")
            {
                prin = new ClaimsPrincipal();
            }
            else
            {
                prin = await AuthorityLocal.GetPrincipalAsync(code, null, cancellationToken);
            }
            var accessToken = await AuthorityLocal.CreateAccessTokenAsync((ClaimsIdentity)prin.Identity, null, cancellationToken);
            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
                ExpiresIn = accessToken.ExpiresIn,
                TokenType = accessToken.TokenType
            };
        }

        private async Task<TokenResponse> OAuth2TokenPassword(string clientId, string clientSecret, string username, string password, IEnumerable<string> scopes, CancellationToken cancellationToken)
        {
            var claimsIdentity = new System.Security.Claims.ClaimsIdentity();
            claimsIdentity.AddClaim(new System.Security.Claims.Claim("client_id", clientId));
            claimsIdentity.AddClaim(new System.Security.Claims.Claim("sub", username));
            claimsIdentity.AddClaim(new System.Security.Claims.Claim(ClaimsIdentity.DefaultNameClaimType, username));
            foreach (var scope in scopes)
            {
                claimsIdentity.AddClaim(new System.Security.Claims.Claim("scope", scope));
            }
            var accessToken = await AuthorityLocal.CreateAccessTokenAsync(claimsIdentity, null, cancellationToken);
            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
                ExpiresIn = accessToken.ExpiresIn,
                Scope = string.Join(" ", scopes),
                TokenType = accessToken.TokenType
            };
        }

        private async Task<TokenResponse> OAuth2TokenClientCredentials(string clientId, string clientSecret, IEnumerable<string> scopes, CancellationToken cancellationToken)
        {
            var claimsIdentity = new System.Security.Claims.ClaimsIdentity();
            claimsIdentity.AddClaim(new System.Security.Claims.Claim("client_id", clientId));
            claimsIdentity.AddClaim(new System.Security.Claims.Claim(ClaimsIdentity.DefaultNameClaimType, clientId));
            foreach(var scope in scopes)
            {
                claimsIdentity.AddClaim(new System.Security.Claims.Claim("scope", scope));
            }
            var accessToken = await AuthorityLocal.CreateAccessTokenAsync(claimsIdentity, null, cancellationToken);
            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
                ExpiresIn = accessToken.ExpiresIn,
                Scope = string.Join(" ", scopes),
                TokenType = accessToken.TokenType
            };
        }

        public async Task<WebContent> OAuth2AuthorizeGet(IEnumerable<string> scope, string responseType, string clientId, string redirectUri = null, string state = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            //return Task.FromResult(new WebContent()
            //{
            //    Content = new MemoryStream(Encoding.UTF8.GetBytes("Please authenticate!")),
            //    ContentType = "text/plain",
            //    CharSet = Encoding.UTF8.EncodingName
            //});
            if (string.IsNullOrEmpty(redirectUri)) throw new ArgumentNullException(nameof(redirectUri));
            if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrEmpty(responseType)) throw new ArgumentNullException(nameof(responseType));
            var ci = new ClaimsIdentity(new[] { new System.Security.Claims.Claim("AllowedPath", "/*")});
            var idToken = await AuthorityLocal.CreateAccessTokenAsync(ci, null, cancellationToken);
            var session_state = $"&session_state={Guid.NewGuid()}";
            state = string.IsNullOrEmpty(state) ? "" : $"&state={HttpUtility.UrlEncode(state)}";
            return new WebContent()
            {
                Location = $"{redirectUri}?{responseType}={idToken.AccessToken}{session_state}{state}"
            };
        }

        public Task<WebContent> OAuth2AuthorizePost(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new WebContent()
            {
                Content = new MemoryStream(Encoding.UTF8.GetBytes("Please authenticate!")),
                ContentType = "text/plain",
                CharSet = Encoding.UTF8.EncodingName
            });
        }
    }
}
