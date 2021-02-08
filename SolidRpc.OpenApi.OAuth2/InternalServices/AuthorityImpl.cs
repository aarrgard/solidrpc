using Microsoft.IdentityModel.Tokens;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Types.OAuth2;
using System;
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
        private class AuthorityTokenValidationParameters : TokenValidationParameters, IAuthorityTokenChecks
        {
            public AuthorityTokenValidationParameters(string issuer, IEnumerable<SecurityKey> allSigningKeys)
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
        }
        private IAuthorityFactory AuthorityFactoryImpl { get; }
        private IHttpClientFactory HttpClientFactory { get; }
        private ISerializerFactory SerializerFactory { get; }

        /// <summary>
        /// The authority
        /// </summary>
        public string Authority { get; }
        
        private OpenIDConnnectDiscovery OpenIDConnnectDiscovery { get; set; }

        /// <summary>
        /// All the signing keys
        /// </summary>
        protected OpenIDKeys OpenIdKeys { get; set; }

        /// <summary>
        /// Return the client jwt.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> GetClientJwtAsync(string clientId, string clientSecret, CancellationToken cancellationToken = default(CancellationToken))
        {
            var doc = await GetDiscoveryDocumentAsync(cancellationToken);
            var client = HttpClientFactory.CreateClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            nvc.Add(new KeyValuePair<string, string>("client_id", clientId));
            nvc.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
            nvc.Add(new KeyValuePair<string, string>("scope", "api"));
            var content = new FormUrlEncodedContent(nvc);

            var resp = await client.PostAsync(doc.TokenEndpoint, content);
            TokenResponse result;
            using (var s = await resp.Content.ReadAsStreamAsync())
            {
                SerializerFactory.DeserializeFromStream(s, out result);
            }

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
            var resp = await client.GetAsync(new Uri(new Uri(Authority), ".well-known/openid-configuration"));
            using (var s = await resp.Content.ReadAsStreamAsync())
            {
                SerializerFactory.DeserializeFromStream(s, out openIDConnnectDiscovery);
            }
            OpenIDConnnectDiscovery = openIDConnnectDiscovery;
            OpenIdKeys = null;
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
            var tokenValidationParameter = new AuthorityTokenValidationParameters(Authority, allSigningKeys);
            tokenChecks?.Invoke(tokenValidationParameter);
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var claimsPrincipal = tokenHandler.ValidateToken(jwt, tokenValidationParameter, out securityToken);
            return claimsPrincipal;
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
        public async Task<IEnumerable<OpenIDKey>> GetSigningKeysAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var openIdKeys = OpenIdKeys;
            if (openIdKeys != null)
            {
                return openIdKeys.Keys;
            }

            var localKeys = GetLocalKeys();
            if(localKeys.Any())
            {
                openIdKeys = new OpenIDKeys() { Keys = localKeys };
            }
            else
            {
                var doc = await GetDiscoveryDocumentAsync(cancellationToken);
                var client = HttpClientFactory.CreateClient();
                var json = await client.GetStringAsync(doc.JwksUri);
                SerializerFactory.DeserializeFromString(json, out openIdKeys);
            }
            OpenIdKeys = openIdKeys;
            return openIdKeys.Keys;
        }

        protected virtual IEnumerable<OpenIDKey> GetLocalKeys()
        {
            return new OpenIDKey[0];
        }
    }
}
