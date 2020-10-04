using Microsoft.Extensions.Logging;
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
        /// <param name="logger"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="serializerFactory"></param>
        public AuthorityFactoryImpl(
            ILogger<AuthorityFactoryImpl> logger,
            IHttpClientFactory httpClientFactory,
            ISerializerFactory serializerFactory)
        {
            Logger = logger;
            HttpClientFactory = httpClientFactory;
            SerializerFactory = serializerFactory;
            Authorities = new ConcurrentDictionary<Uri, IAuthority>();
        }
        private ILogger Logger { get; }
        private IHttpClientFactory HttpClientFactory { get; }
        private ISerializerFactory SerializerFactory { get; }
        private ConcurrentDictionary<Uri, IAuthority> Authorities { get; }

        /// <summary>
        /// Returns the authority
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IAuthority GetAuthority(Uri url)
        {
            return Authorities.GetOrAdd(url, CreateAuthority);
        }

        private IAuthority CreateAuthority(Uri authority)
        {
            return new AuthorityImpl(this, HttpClientFactory, SerializerFactory, authority);
        }
    }
}
