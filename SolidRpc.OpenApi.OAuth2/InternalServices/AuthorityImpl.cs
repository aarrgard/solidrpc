﻿using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Types.OAuth2;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.OAuth2.InternalServices
{
    /// <summary>
    /// Represents an authority
    /// </summary>
    public class AuthorityImpl : IAuthority
    {
        private class CachedJwt
        {
            public CachedJwt(TokenResponse jwt, DateTime validTo)
            {
                Jwt = jwt;
                ValidTo = validTo;
            }
            public TokenResponse Jwt { get; }
            public DateTime ValidTo { get; }
        }

        private class AuthorityTokenValidationParameters : TokenValidationParameters, IAuthorityTokenChecks
        {
            public AuthorityTokenValidationParameters(string issuer, IEnumerable<SecurityKey> allSigningKeys, Action keyMissingAction)
            {
                ValidateActor = false;
                ValidateAudience = false;
                ValidIssuer = issuer;
                ValidateIssuer = true;
                ValidateIssuerSigningKey = true;
                ValidateLifetime = true;
                RequireExpirationTime = true;
                RequireSignedTokens = true;
                RequireAudience = false;
                IssuerSigningKeyResolver = (token, st, kid, validationParameters) => {
                    var signingKeys = allSigningKeys.Where(o => o.KeyId == kid).ToList();
                    if(signingKeys.Count == 0)
                    {
                        keyMissingAction();
                    }
                    return signingKeys;
                };
            }
            
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="authorityFactoryImpl"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="serializerFactory"></param>
        /// <param name="authority"></param>
        public AuthorityImpl(
            ILogger<AuthorityImpl> logger,
            IAuthorityFactory authorityFactoryImpl,
            IHttpClientFactory httpClientFactory,
            ISerializerFactory serializerFactory,
            string authority)
        {
            Logger = logger;
            AuthorityFactoryImpl = authorityFactoryImpl;
            SerializerFactory = serializerFactory;
            HttpClientFactory = httpClientFactory;
            Authority = authority;
            CachedJwts = new ConcurrentDictionary<string, CachedJwt>();
            GrantTypeScopes = new Dictionary<string, IEnumerable<string>>();
        }
        private ILogger Logger { get; }
        private IAuthorityFactory AuthorityFactoryImpl { get; }
        private IHttpClientFactory HttpClientFactory { get; }
        private ISerializerFactory SerializerFactory { get; }
        private ConcurrentDictionary<string, CachedJwt> CachedJwts { get; }
        private Dictionary<string, IEnumerable<string>> GrantTypeScopes { get; }

        /// <summary>
        /// The authority
        /// </summary>
        public string Authority { get; }

        private OpenIDConnectDiscovery OpenIDConnnectDiscovery { get; set; }

        /// <summary>
        /// All the signing keys
        /// </summary>
        protected OpenIDKeys OpenIdKeys { get; set; }

        internal Task PruneCachedJwts()
        {
            var now = DateTime.UtcNow;
            foreach (var cachedJwt in CachedJwts)
            {
                if(cachedJwt.Value.ValidTo < now)
                {
                    CachedJwts.TryRemove(cachedJwt.Key, out CachedJwt tmp);
                }
            }
            return Task.CompletedTask;
        }

        public void AddDefaultScopes(string grantType, IEnumerable<string> scopes)
        {
            if (GrantTypeScopes.TryGetValue(grantType, out IEnumerable<string> additionalScopes))
            {
                GrantTypeScopes[grantType] = additionalScopes.Union(scopes).Distinct().ToList();
            }
            else
            {
                GrantTypeScopes[grantType] = scopes.ToList();
            }
        }

        public IEnumerable<string> GetScopes(string grantType, IEnumerable<string> scopes)
        {
            if(scopes == null)
            {
                scopes = new string[0];
            }
            if(GrantTypeScopes.TryGetValue(grantType, out IEnumerable<string> additionalScopes))
            {
                scopes = additionalScopes.Union(scopes).Distinct();
            }
            return scopes;
        }
        private string CreateScopes(string grantType, IEnumerable<string> scopes)
        {
            return string.Join(" ", GetScopes(grantType, scopes));
        }

        /// <summary>
        /// Return the client jwt.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="scopes"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TokenResponse> GetClientJwtAsync(string clientId, string clientSecret, IEnumerable<string> scopes, TimeSpan? timeout, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            nvc.Add(new KeyValuePair<string, string>("client_id", clientId));
            nvc.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
            nvc.Add(new KeyValuePair<string, string>("scope", CreateScopes("client_credentials", scopes)));
            return GetJwtAsync(nvc, timeout, cancellationToken);
        }

        /// <summary>
        /// Returns the user JWT
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="scopes"></param>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TokenResponse> GetUserJwtAsync(string clientId, string clientSecret, string username, string password, IEnumerable<string> scopes, TimeSpan? timeout, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "password"));
            nvc.Add(new KeyValuePair<string, string>("client_id", clientId));
            nvc.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
            nvc.Add(new KeyValuePair<string, string>("username", username));
            nvc.Add(new KeyValuePair<string, string>("password", password));
            nvc.Add(new KeyValuePair<string, string>("scope", CreateScopes("password", scopes)));

            return GetJwtAsync(nvc, timeout, cancellationToken);
        }

        public Task<TokenResponse> GetCodeJwtToken(string clientId, string clientSecret, string code, string redirectUri, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            nvc.Add(new KeyValuePair<string, string>("client_id", clientId));
            nvc.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
            nvc.Add(new KeyValuePair<string, string>("code", code));
            nvc.Add(new KeyValuePair<string, string>("redirect_uri", redirectUri));
            return GetJwtAsync(nvc, timeout, cancellationToken);
        }

        public Task<TokenResponse> RefreshTokenAsync(string clientId, string clientSecret, string refreshToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
            nvc.Add(new KeyValuePair<string, string>("client_id", clientId));
            nvc.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
            nvc.Add(new KeyValuePair<string, string>("refresh_token", refreshToken));
            return GetTokenResponseAsync(nvc, cancellationToken);
        }

        private async Task<TokenResponse> GetJwtAsync(List<KeyValuePair<string, string>> nvc, TimeSpan? timeout, CancellationToken cancellationToken)
        {
            // defalt timeout of 5 minutes...
            if (timeout == null) timeout = TimeSpan.FromMinutes(5);

            var key = string.Join(":", nvc.Select(o => o.Value));
            if(CachedJwts.TryGetValue(key, out CachedJwt cachedJwt))
            {
                if (cachedJwt.ValidTo < DateTime.Now.Subtract(timeout.Value))
                {
                    return cachedJwt.Jwt;
                }
            }

            var result = await GetTokenResponseAsync(nvc, cancellationToken);
            if(result == null)
            {
                return null;
            }

            //
            // parse returned token
            //
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(result.AccessToken);

            //
            // add cached token
            //
            var cj = new CachedJwt(result, securityToken.ValidTo);
            CachedJwts.AddOrUpdate(key, cj, (k, o) => cj);

            return result;
        }

        private async Task<TokenResponse> GetTokenResponseAsync(List<KeyValuePair<string, string>> nvc, CancellationToken cancellationToken)
        {
            var doc = await GetDiscoveryDocumentAsync(cancellationToken);
            var client = HttpClientFactory.CreateClient();
            var content = new FormUrlEncodedContent(nvc);
            var resp = await client.PostAsync(doc.TokenEndpoint, content);

            if (!resp.IsSuccessStatusCode)
            {
                Logger.LogError($"Response from {doc.TokenEndpoint} is not successful.");
                return null;
            }

            TokenResponse result = null;
            try
            {
                var strResp = await resp.Content.ReadAsStringAsync();
                SerializerFactory.DeserializeFromString(strResp, out result);
            }
            catch
            {
                Logger.LogError("Failed to deserialize response from authority.");
                return null;
            }

            if (string.IsNullOrEmpty(result?.AccessToken))
            {
                Logger.LogError("Response from authority does not contain an access token");
                return null;
            }
            return result;
        }

        /// <summary>
        /// Returns the discovery document for this authority
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<OpenIDConnectDiscovery> GetDiscoveryDocumentAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            //
            // use cached version
            //
            var openIDConnnectDiscovery = OpenIDConnnectDiscovery;
            if (openIDConnnectDiscovery != null)
            {
                return openIDConnnectDiscovery;
            }

            //
            // fetch new version
            //
            var client = HttpClientFactory.CreateClient();
            var separator = Authority.EndsWith("/") ? "" : "/";
            var uri = new Uri($"{Authority}{separator}.well-known/openid-configuration");
            var resp = await client.GetAsync(uri);
            if(!resp.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot find well known configuration @ {uri}");
            }
            using (var s = await resp.Content.ReadAsStreamAsync())
            {
                SerializerFactory.DeserializeFromStream(s, out openIDConnnectDiscovery);
            }
            OpenIDConnnectDiscovery = openIDConnnectDiscovery;
            return openIDConnnectDiscovery;
        }

        /// <summary>
        /// Returns the principal for supplied jwt token.
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="tokenChecks"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ClaimsPrincipal> GetPrincipalAsync(string jwt, Action<IAuthorityTokenChecks> tokenChecks = null,  CancellationToken cancellationToken = default(CancellationToken))
        {
            var allSigningKeys = await GetSecuritySigningKeysAsync(cancellationToken);
            var tokenValidationParameter = new AuthorityTokenValidationParameters(Authority, allSigningKeys, ReloadSigningKeys);
            tokenChecks?.Invoke(tokenValidationParameter);
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var claimsPrincipal = tokenHandler.ValidateToken(jwt, tokenValidationParameter, out securityToken);
            claimsPrincipal.Identities.First().AddClaim(new Claim("accesstoken", jwt));
            return claimsPrincipal;
        }

        private async void ReloadSigningKeys()
        {
            await GetSigningKeysNoCacheAsync(CancellationToken.None);
        }

        /// <summary>
        /// Returns the signing keys
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<IEnumerable<SecurityKey>> GetSecuritySigningKeysAsync(CancellationToken cancellationToken)
        {
            var openIdKeys = await GetSigningKeysAsync(cancellationToken);
            return openIdKeys.Select(o => o.AsSecurityKey());
        }

        /// <summary>
        /// Returns the signing keys
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IEnumerable<OpenIDKey>> GetSigningKeysAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var openIdKeys = OpenIdKeys;
            if (openIdKeys != null)
            {
                return Task.FromResult(openIdKeys.Keys);
            }

            return GetSigningKeysNoCacheAsync(cancellationToken);
        }
        /// <summary>
        /// Returns the signing keys
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<IEnumerable<OpenIDKey>> GetSigningKeysNoCacheAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var doc = await GetDiscoveryDocumentAsync(cancellationToken);
            var client = HttpClientFactory.CreateClient();
            var json = await client.GetStringAsync(doc.JwksUri);
            SerializerFactory.DeserializeFromString(json, out OpenIDKeys openIdKeys);

            OpenIdKeys = openIdKeys;
            return openIdKeys.Keys;
        }

        public async Task RevokeTokenAsync(string clientId, string clientSecret, string token, CancellationToken cancellationToken = default)
        {
            var doc = await GetDiscoveryDocumentAsync(cancellationToken);
            var client = HttpClientFactory.CreateClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("client_id", clientId));
            nvc.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
            nvc.Add(new KeyValuePair<string, string>("token", token));
            var content = new FormUrlEncodedContent(nvc);
            var resp = await client.PostAsync(doc.TokenEndpoint, content);

            if (!resp.IsSuccessStatusCode)
            {
                Logger.LogError($"Response from {doc.TokenEndpoint} authority is not succesful.");
            }

        }
    }
}
