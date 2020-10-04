using Microsoft.IdentityModel.Tokens;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Types.OAuth2;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SolidRpc.OpenApi.OAuth2.InternalServices
{
    /// <summary>
    /// Represents an authority
    /// </summary>
    public class AuthorityImpl : IAuthority
    {
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
            Uri authority)
        {
            AuthorityFactoryImpl = authorityFactoryImpl;
            SerializerFactory = serializerFactory;
            HttpClientFactory = httpClientFactory;
            Authority = authority;
        }
        private IAuthorityFactory AuthorityFactoryImpl { get; }
        private IHttpClientFactory HttpClientFactory { get; }
        private ISerializerFactory SerializerFactory { get; }
        private Uri Authority { get; }
        
        private OpenIDConnnectDiscovery OpenIDConnnectDiscovery { get; set; }

        private IEnumerable<RsaSecurityKey> Keys { get; set; }

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
            var resp = await client.GetAsync(new Uri(Authority, ".well-known/openid-configuration"));
            OpenIDConnnectDiscovery result;
            using (var s = await resp.Content.ReadAsStreamAsync())
            {
                SerializerFactory.DeserializeFromStream(s, out result);
            }
            openIDConnnectDiscovery = result;
            return openIDConnnectDiscovery;
        }

        /// <summary>
        /// Returns the principal for supplied jwt token.
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPrincipal> GetPrincipalAsync(string jwt, CancellationToken cancellationToken = default(CancellationToken))
        {
            var doc = await GetDiscoveryDocumentAsync(cancellationToken);
            var allSigningKeys = await GetSigningKeysAsync(cancellationToken);
            var tokenValidationParameter = new TokenValidationParameters()
            {
                ValidateActor = false,
                ValidateAudience = false,
                ValidIssuer = CreateIssuer(doc.Issuer),
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                IssuerSigningKeyResolver = (token, st, kid, validationParameters) => {
                    var signingKeys = allSigningKeys.Where(o => o.KeyId == kid).ToList();
                    return signingKeys;
                }
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var claimsPrincipal = tokenHandler.ValidateToken(jwt, tokenValidationParameter, out securityToken);

            return claimsPrincipal;
        }

        private string CreateIssuer(Uri issuer)
        {
            var iss = issuer.ToString();
            if(iss.EndsWith("/"))
            {
                iss = iss.Substring(0, iss.Length - 1);
            }
            return iss;
        }

        /// <summary>
        /// Returns the signing keys
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SecurityKey>> GetSigningKeysAsync(CancellationToken cancellationToken)
        {
            var keys = Keys;
            if(keys != null)
            {
                return keys;
            }
            var doc = await GetDiscoveryDocumentAsync(cancellationToken);
            var client = HttpClientFactory.CreateClient();
            var json = await client.GetStringAsync(doc.JwksUri);
            var jsonKeys = JsonWebKeySet.Create(json);
            return jsonKeys.GetSigningKeys();
        }
    }
}
