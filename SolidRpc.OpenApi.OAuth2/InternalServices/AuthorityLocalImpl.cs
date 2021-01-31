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
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.OAuth2.InternalServices
{
    /// <summary>
    /// Implements the local authority.
    /// </summary>
    public class AuthorityLocalImpl : AuthorityImpl, IAuthorityLocal
    {
        private SecurityKey _privateKey;
        private SecurityKey _publicKey;

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="authorityFactoryImpl"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="serializerFactory"></param>
        /// <param name="authority"></param>
        public AuthorityLocalImpl(
            IAuthorityFactory authorityFactoryImpl,
            IHttpClientFactory httpClientFactory,
            ISerializerFactory serializerFactory,
            Uri authority)
            :base(authorityFactoryImpl, httpClientFactory, serializerFactory, authority)
        {
        }
        public OpenIDKey PrivateSigningKey => _privateKey.AsOpenIDKey();
        protected override IEnumerable<OpenIDKey> GetLocalKeys()
        {
            if (_publicKey == null) throw new Exception("No signing key exists for local authority.");
            yield return _publicKey.AsOpenIDKey();
        }

        public async Task<TokenResponse> CreateAccessTokenAsync(ClaimsIdentity claimsIdentity, TimeSpan? expiryTime = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if(expiryTime == null)
            {
                expiryTime = TimeSpan.FromHours(1);
            }
            if(_privateKey == null)
            {
                throw new Exception("No private signing key available");
            }
            var keys = await GetSigningKeysAsync(cancellationToken);
            if (!keys.Any(o => o.Kid == _privateKey.KeyId))
            {
                throw new Exception("Private signing key not part o public keys");
            }

            var issuedAt = DateTime.UtcNow;
            var signingCredentials = new SigningCredentials(_privateKey, SecurityAlgorithms.RsaSha512Signature);
            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = CreateIssuer(Authority.ToString()),
                Expires = issuedAt.Add(expiryTime.Value),
                IssuedAt = issuedAt,
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials,
                //EncryptingCredentials = encryptingCredentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var plainToken = (JwtSecurityToken)tokenHandler.CreateToken(securityTokenDescriptor);
            return new TokenResponse()
            {
                AccessToken = plainToken.RawData,
                ExpiresIn = ((int)(plainToken.ValidTo - DateTime.UtcNow).TotalSeconds).ToString(),
                TokenType = "jwt"
            };
        }

        public void CreateSigningKey()
        {
            var rsa = RSA.Create();
            var keyId = Guid.NewGuid().ToString();
            var privateKey = new RsaSecurityKey(rsa.ExportParameters(true))
            {
                KeyId = keyId
            };
            var publicKey = new RsaSecurityKey(rsa.ExportParameters(false))
            {
                KeyId = keyId
            };
            SetSigningKey(privateKey, publicKey);
        }

        private void SetSigningKey(SecurityKey privateKey, SecurityKey publicKey)
        {
            _privateKey = privateKey;
            _publicKey = publicKey;
        }

        public void SetSigningKey(X509Certificate2 cert)
        {
            SetSigningKey(new RsaSecurityKey(cert.GetRSAPrivateKey()), new RsaSecurityKey(cert.GetRSAPublicKey()));

        }
    }
}
