using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Serialization;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.OAuth2.InternalServices
{

    /// <summary>
    /// Implements the authority
    /// </summary>
    public class AuthorityFactoryImpl : IAuthorityFactory
    {
        /// <summary>
        /// represents som jwt data
        /// </summary>
        private class JwtData
        {
            /// <summary>
            /// The issuer
            /// </summary>
            [DataMember(Name = "iss")]
            public string Iss { get; set; }

            /// <summary>
            /// The issuer
            /// </summary>
            [DataMember(Name = "exp")]
            public long Exp { get; set; }
        }

        private static string Base64DecodeToString(string ToDecode)
        {
            string decodePrepped = ToDecode.Replace("-", "+").Replace("_", "/");

            switch (decodePrepped.Length % 4)
            {
                case 0:
                    break;
                case 2:
                    decodePrepped += "==";
                    break;
                case 3:
                    decodePrepped += "=";
                    break;
                default:
                    throw new Exception("Not a legal base64 string!");
            }

            byte[] data = Convert.FromBase64String(decodePrepped);
            return System.Text.Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="serializerFactory"></param>
        /// <param name="authorityConfigurator"></param>
        public AuthorityFactoryImpl(
            ILogger<AuthorityImpl> logger,
            IHttpClientFactory httpClientFactory,
            ISerializerFactory serializerFactory,
            AuthorityConfigurator authorityConfigurator)
        {
            Logger = logger;
            HttpClientFactory = httpClientFactory;
            SerializerFactory = serializerFactory;
            AuthorityConfigurator = authorityConfigurator;
            Authorities = new ConcurrentDictionary<string, AuthorityImpl>();
            LocalAuthorities = new ConcurrentDictionary<string, IAuthorityLocal>();
            PruneCachedJwts();
        }
        private ILogger<AuthorityImpl> Logger { get; }
        private IHttpClientFactory HttpClientFactory { get; }
        private ISerializerFactory SerializerFactory { get; }
        private AuthorityConfigurator AuthorityConfigurator { get; }
        private ConcurrentDictionary<string, AuthorityImpl> Authorities { get; }
        private ConcurrentDictionary<string, IAuthorityLocal> LocalAuthorities { get; }

        private async void PruneCachedJwts()
        {
            while(true)
            {
                foreach (var localAuth in Authorities.Values)
                {
                    await localAuth.PruneCachedJwts();
                }
                await Task.Delay(60 * 1000);
            }
        }

        /// <summary>
        /// Returns the authority
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IAuthority GetAuthority(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;
            return Authorities.GetOrAdd(url, _ => AuthorityConfigurator.Configure(new AuthorityImpl(Logger, this, HttpClientFactory, SerializerFactory, url)));
        }

        /// <summary>
        /// Returns the local authority
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IAuthorityLocal GetLocalAuthority(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));
            var authorityImpl = (AuthorityImpl)GetAuthority(url);
            return LocalAuthorities.GetOrAdd(url, _ => new AuthorityLocalImpl(authorityImpl));

        }

        public Task<ClaimsPrincipal> GetPrincipalAsync(string jwt, Action<IAuthorityTokenChecks> tokenChecks = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(jwt)) throw new ArgumentNullException(nameof(jwt));
            var parts = jwt.Split('.');
            if (parts.Length < 2) throw new ArgumentException("jwt is not well formed");
            SerializerFactory.DeserializeFromString(Base64DecodeToString(parts[1]), out JwtData data);
            if(string.IsNullOrEmpty(data?.Iss)) throw new ArgumentException("jwt does not contain an issuer");
            return GetAuthority(data.Iss).GetPrincipalAsync(jwt, tokenChecks, cancellationToken);
        }
    }
}
