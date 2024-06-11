using Microsoft.IdentityModel.Tokens;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Types.OAuth2;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
    public class AuthorityLocalImpl : IAuthorityLocal
    {
        private class RsaKeyPair
        {
            public RsaKeyPair(RsaKey publicKey, RsaKey privateKey, DateTimeOffset expires)
            {
                PublicKey = publicKey;
                PrivateKey = privateKey;
                Expires = expires;
            }

            public RsaKey PublicKey { get; }
            public RsaKey PrivateKey { get; }
            public DateTimeOffset Expires { get; }

            public string KeyId => PublicKey.KeyId;
        }

        private class RsaKey
        {
            public RsaKey(RSA rsa, string keyId)
            {
                RSA = rsa;
                SecurityKey = new RsaSecurityKey(rsa) { KeyId = keyId};
                OpenIDKey = SecurityKey.AsOpenIDKey();
            }
            public string KeyId => SecurityKey.KeyId;
            public RSA RSA { get; }
            public SecurityKey SecurityKey { get; }
            public OpenIDKey OpenIDKey { get; }
        }

        private IList<RsaKeyPair> _keys;

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="authorityImpl"></param>
        public AuthorityLocalImpl(
            AuthorityImpl authorityImpl)
        {
            AuthorityImpl = authorityImpl;
            _keys = new List<RsaKeyPair>();
        }

        /// <summary>
        /// 
        /// </summary>
        public AuthorityLocalImpl()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public IAuthorityFactory AuthorityFactory => AuthorityImpl.AuthorityFactory;

        private AuthorityImpl AuthorityImpl { get; }

        private RsaKeyPair CurrentKeyPair => _keys.First();

        /// <summary>
        /// Returns the private signing key.
        /// </summary>
        public OpenIDKey PrivateSigningKey => CurrentKeyPair.PrivateKey.OpenIDKey;

        /// <summary>
        /// Returns the public signing key
        /// REturns the public signing key
        /// </summary>
        public OpenIDKey PublicSigningKey => CurrentKeyPair.PublicKey.OpenIDKey;

        /// <summary>
        /// Returns the authority.
        /// </summary>
        public string Authority => AuthorityImpl.Authority;

        /// <summary>
        /// Returns the local keys
        /// </summary>
        /// <returns></returns>
        private IEnumerable<OpenIDKey> GetLocalKeys()
        {
            return _keys.Select(o => o.PublicKey.OpenIDKey);
        }

        /// <summary>
        /// Creates the access token
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <param name="expires"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TokenResponse> CreateAccessTokenAsync(ClaimsIdentity claimsIdentity, DateTimeOffset? expires = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var issuedAt = DateTimeOffset.UtcNow;
            if (expires == null)
            {
                expires = issuedAt.AddHours(1);
            }
            if(!_keys.Any())
            {
                throw new Exception("No private signing key available");
            }
            var secKey = CurrentKeyPair.PrivateKey.SecurityKey;
            var keys = await GetSigningKeysAsync(cancellationToken);
            if (!keys.Any(o => o.Kid == secKey.KeyId))
            {
                throw new Exception("Private signing key not part o public keys");
            }

            var signingCredentials = new SigningCredentials(secKey, SecurityAlgorithms.RsaSha512Signature);
            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = Authority,
                Expires = expires.Value.UtcDateTime,
                IssuedAt = issuedAt.UtcDateTime,
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials,
                //EncryptingCredentials = encryptingCredentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var plainToken = (JwtSecurityToken)tokenHandler.CreateToken(securityTokenDescriptor);
            return new TokenResponse()
            {
                AccessToken = plainToken.RawData,
                RefreshToken = plainToken.RawData,
                ExpiresIn = ((int)(plainToken.ValidTo - DateTime.UtcNow).TotalSeconds).ToString(),
                TokenType = "jwt",
            };
        }

        /// <summary>
        /// Creates a signing key
        /// </summary>
        public void CreateSigningKey(bool removeOld = false)
        {
            var privateRSA = RSA.Create();
            var publicRSA = RSA.Create();
            publicRSA.ImportParameters(privateRSA.ExportParameters(false));
            SetRsa(publicRSA, privateRSA, Guid.NewGuid().ToString(), DateTimeOffset.MaxValue);
        }

        private RsaKeyPair SetRsa(
            RSA publicKey, 
            RSA privateKey,
            string keyId,
            DateTimeOffset expires)
        {
            var keyPair = new RsaKeyPair(new RsaKey(publicKey, keyId), new RsaKey(privateKey, keyId), expires);
            _keys = _keys.Union(new [] { keyPair }).OrderByDescending(o => o.Expires).ToList();
            return keyPair;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cert"></param>
        /// <param name="keyId"></param>
        public void AddSigningKey(X509Certificate2 cert, Func<X509Certificate2, string> keyId = null)
        {
            if (keyId == null) keyId = c => c.Thumbprint;
            SetRsa(cert.GetRSAPublicKey(), cert.GetRSAPrivateKey(), keyId(cert), cert.NotAfter);
        }

        /// <summary>
        /// Sets the signing key.
        /// </summary>
        /// <param name="cert"></param>
        /// <param name="keyId"></param>
        public void SetSigningKey(X509Certificate2 cert, Func<X509Certificate2, string> keyId)
        {
            if (keyId == null) keyId = c => c.Thumbprint;
            var keypair = SetRsa(cert.GetRSAPublicKey(), cert.GetRSAPrivateKey(), keyId(cert), cert.NotAfter);
            _keys = _keys.OrderBy(o => keypair.KeyId == o.KeyId ? 0 : 1).ToList();
        }

        Task<OpenIDConnectDiscovery> IAuthority.GetDiscoveryDocumentAsync(CancellationToken cancellationToken)
        {
            return AuthorityImpl.GetDiscoveryDocumentAsync(cancellationToken);
        }

        Task<ClaimsPrincipal> IAuthority.GetPrincipalAsync(string jwt, Action<IAuthorityTokenChecks> tokenChecks, CancellationToken cancellationToken)
        {
            return AuthorityImpl.GetPrincipalAsync(jwt, tokenChecks, cancellationToken);
        }
        /// <summary>
        /// Returns the signing keys
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IEnumerable<OpenIDKey>> GetSigningKeysAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetLocalKeys());
        }

        Task<TokenResponse> IAuthority.GetClientJwtAsync(string clientId, string clientSecret, IEnumerable<string> scopes, TimeSpan? timeout, CancellationToken cancellationToken)
        {
            return AuthorityImpl.GetClientJwtAsync(clientId, clientSecret, scopes, timeout, cancellationToken);
        }

        Task<TokenResponse> IAuthority.GetUserJwtAsync(string clientId, string clientSecret, string username, string password, IEnumerable<string> scopes, TimeSpan? timeout, CancellationToken cancellationToken)
        {
            return AuthorityImpl.GetUserJwtAsync(clientId, clientSecret, username, password, scopes, timeout, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="code"></param>
        /// <param name="redirectUri"></param>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TokenResponse> GetCodeJwtToken(string clientId, string clientSecret, string code, string redirectUri, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            return AuthorityImpl.GetCodeJwtToken(clientId, clientSecret, code, redirectUri, timeout, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="refreshToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TokenResponse> RefreshTokenAsync(string clientId, string clientSecret, string refreshToken, CancellationToken cancellationToken)
        {
            return AuthorityImpl.RefreshTokenAsync(clientId, clientSecret, refreshToken, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grantType"></param>
        /// <param name="scopes"></param>
        public void AddDefaultScopes(string grantType, IEnumerable<string> scopes)
        {
            AuthorityImpl.AddDefaultScopes(grantType, scopes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grantType"></param>
        /// <param name="additionalScopes"></param>
        /// <returns></returns>
        public IEnumerable<string> GetScopes(string grantType, IEnumerable<string> additionalScopes)
        {
            return AuthorityImpl.GetScopes(grantType, additionalScopes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="token"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task RevokeTokenAsync(string clientId, string clientSecret, string token, CancellationToken cancellationToken = default)
        {
            return AuthorityImpl.RevokeTokenAsync(clientId, clientSecret, token, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<byte[]> SignHash(byte[] data, CancellationToken cancellationToken = default)
        {
            var b = CurrentKeyPair.PrivateKey.RSA.SignData(data, 0, data.Length, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

            return Task.FromResult(b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> VerifyData(byte[] data, byte[] signature, CancellationToken cancellationToken = default)
        {
            return AuthorityImpl.VerifyData(data, signature, cancellationToken);
        }
    }
}
