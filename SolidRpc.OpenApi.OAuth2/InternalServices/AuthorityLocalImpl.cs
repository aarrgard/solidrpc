﻿using Microsoft.IdentityModel.Tokens;
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
        private SecurityKey _privateKey;
        private SecurityKey _publicKey;

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="authorityImpl"></param>
        public AuthorityLocalImpl(
            AuthorityImpl authorityImpl)
        {
            AuthorityImpl = authorityImpl;
        }

        private AuthorityImpl AuthorityImpl { get; }

        /// <summary>
        /// Returns the private signing key.
        /// </summary>
        OpenIDKey IAuthorityLocal.PrivateSigningKey => _privateKey.AsOpenIDKey();

        public string Authority => AuthorityImpl.Authority;

        /// <summary>
        /// Returns the local keys
        /// </summary>
        /// <returns></returns>
        private IEnumerable<OpenIDKey> GetLocalKeys()
        {
            if (_publicKey == null) throw new Exception("No signing key exists for local authority.");
            yield return _publicKey.AsOpenIDKey();
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
            if(_privateKey == null)
            {
                throw new Exception("No private signing key available");
            }
            var keys = await GetSigningKeysAsync(cancellationToken);
            if (!keys.Any(o => o.Kid == _privateKey.KeyId))
            {
                throw new Exception("Private signing key not part o public keys");
            }

            var signingCredentials = new SigningCredentials(_privateKey, SecurityAlgorithms.RsaSha512Signature);
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

        /// <summary>
        /// Sets the signing key.
        /// </summary>
        /// <param name="cert"></param>
        public void SetSigningKey(X509Certificate2 cert, Func<X509Certificate2, string> keyId)
        {
            if (keyId == null) keyId = c => c.Thumbprint;
            SetSigningKey(new RsaSecurityKey(cert.GetRSAPrivateKey())
            {
                KeyId = keyId(cert)
            }, new RsaSecurityKey(cert.GetRSAPublicKey())
            {
                KeyId = keyId(cert)
            });

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

        public Task<TokenResponse> GetCodeJwtToken(string clientId, string clientSecret, string code, string redirectUri, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            return AuthorityImpl.GetCodeJwtToken(clientId, clientSecret, code, redirectUri, timeout, cancellationToken);
        }

        public Task<TokenResponse> RefreshTokenAsync(string clientId, string clientSecret, string refreshToken, CancellationToken cancellationToken)
        {
            return AuthorityImpl.RefreshTokenAsync(clientId, clientSecret, refreshToken, cancellationToken);
        }

        public void AddDefaultScopes(string grantType, IEnumerable<string> scopes)
        {
            AuthorityImpl.AddDefaultScopes(grantType, scopes);
        }

        public IEnumerable<string> GetScopes(string grantType, IEnumerable<string> additionalScopes)
        {
            return AuthorityImpl.GetScopes(grantType, additionalScopes);
        }

        public Task RevokeTokenAsync(string clientId, string clientSecret, string token, CancellationToken cancellationToken = default)
        {
            return AuthorityImpl.RevokeTokenAsync(clientId, clientSecret, token, cancellationToken);
        }
    }
}
