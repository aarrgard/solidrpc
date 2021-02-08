using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Serialization;
using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace SolidRpc.OpenApi.OAuth2.InternalServices
{
    /// <summary>
    /// Implements the authority
    /// </summary>
    public class AuthorityFactoryImpl : IAuthorityFactory
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="serializerFactory"></param>
        public AuthorityFactoryImpl(
            IHttpClientFactory httpClientFactory,
            ISerializerFactory serializerFactory)
        {
            HttpClientFactory = httpClientFactory;
            SerializerFactory = serializerFactory;
            Authorities = new ConcurrentDictionary<string, IAuthority>();
            LocalAuthorities = new ConcurrentDictionary<string, IAuthorityLocal>();
        }
        private IHttpClientFactory HttpClientFactory { get; }
        private ISerializerFactory SerializerFactory { get; }
        private ConcurrentDictionary<string, IAuthority> Authorities { get; }
        private ConcurrentDictionary<string, IAuthorityLocal> LocalAuthorities { get; }

        /// <summary>
        /// Returns the authority
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IAuthority GetAuthority(string url)
        {
            return Authorities.GetOrAdd(url, _ => new AuthorityImpl(this, HttpClientFactory, SerializerFactory, url));
        }

        /// <summary>
        /// Returns the local authority
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IAuthorityLocal GetLocalAuthority(string url)
        {
            return LocalAuthorities.GetOrAdd(url, _ => new AuthorityLocalImpl(this, HttpClientFactory, SerializerFactory, url));

        }
    }
}
