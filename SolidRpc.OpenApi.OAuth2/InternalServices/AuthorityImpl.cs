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
            public CachedJwt(string jwt, DateTime validTo)
            {
                Jwt = jwt;
                ValidTo = validTo;
            }
            public string Jwt { get; }
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
        /// <param name="authorityFactoryImpl"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="serializerFactory"></param>
        /// <param name="authority"></param>
        public AuthorityImpl(
            IAuthorityFactory authorityFactoryImpl,
            IHttpClientFactory httpClientFactory,
            ISerializerFactory serializerFactory,
            string authority)
        {
            AuthorityFactoryImpl = authorityFactoryImpl;
            SerializerFactory = serializerFactory;
            HttpClientFactory = httpClientFactory;
            Authority = authority;
            CachedJwts = new ConcurrentDictionary<string, CachedJwt>();
        }
        private IAuthorityFactory AuthorityFactoryImpl { get; }
        private IHttpClientFactory HttpClientFactory { get; }
        private ISerializerFactory SerializerFactory { get; }
        private ConcurrentDictionary<string, CachedJwt> CachedJwts { get; }

        /// <summary>
        /// The authority
        /// </summary>
        public string Authority { get; }

        private OpenIDConnnectDiscovery OpenIDConnnectDiscovery { get; set; }

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

        /// <summary>
        /// Return the client jwt.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="scopes"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetClientJwtAsync(string clientId, string clientSecret, IEnumerable<string> scopes, TimeSpan? timeout, CancellationToken cancellationToken = default(CancellationToken))
        {
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            nvc.Add(new KeyValuePair<string, string>("client_id", clientId));
            nvc.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
            nvc.Add(new KeyValuePair<string, string>("scope", string.Join(" ", scopes)));
            return GetJwtAsync(nvc, timeout, cancellationToken);
        }

        public Task<string> GetUserJwtAsync(string clientId, string clientSecret, string username, string password, IEnumerable<string> scopes, TimeSpan? timeout, CancellationToken cancellationToken = default(CancellationToken))
        {
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "password"));
            nvc.Add(new KeyValuePair<string, string>("client_id", clientId));
            nvc.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
            nvc.Add(new KeyValuePair<string, string>("username", username));
            nvc.Add(new KeyValuePair<string, string>("password", password));
            nvc.Add(new KeyValuePair<string, string>("scope", string.Join(" ", scopes)));

            return GetJwtAsync(nvc, timeout, cancellationToken);
        }

        private async Task<string> GetJwtAsync(List<KeyValuePair<string, string>> nvc, TimeSpan? timeout, CancellationToken cancellationToken)
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
            var doc = await GetDiscoveryDocumentAsync(cancellationToken);
            var client = HttpClientFactory.CreateClient();
            var content = new FormUrlEncodedContent(nvc);
            var resp = await client.PostAsync(doc.TokenEndpoint, content);
            TokenResponse result;
            using (var s = await resp.Content.ReadAsStreamAsync())
            {
                SerializerFactory.DeserializeFromStream(s, out result);
            }

            //
            // parse returned token
            //
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(result.AccessToken);

            //
            // add cached token
            //
            var cj = new CachedJwt(result.AccessToken, securityToken.ValidTo);
            CachedJwts.AddOrUpdate(key, cj, (k, o) => cj);

            return result.AccessToken;
        }

        /// <summary>
        /// Returns the discovery document for this authority
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<OpenIDConnnectDiscovery> GetDiscoveryDocumentAsync(CancellationToken cancellationToken = default(CancellationToken))
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
    }
}
