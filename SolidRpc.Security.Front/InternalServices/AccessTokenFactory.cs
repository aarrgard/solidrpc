using Microsoft.IdentityModel.Tokens;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Security.Front.InternalServices;
using SolidRpc.Security.Services.Oidc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Front.InternalServices
{
    /// <summary>
    /// Implements the jwt token factory
    /// </summary>
    public class AccessTokenFactory : IAccessTokenFactory
    {
        private string _issuer;
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="keyStore"></param>
        public AccessTokenFactory(
            IInvoker<IOidcServer> invoker,
            IOpenIDKeyStore keyStore)
        {
            Invoker = invoker;
            KeyStore = keyStore;
        }

        private IInvoker<IOidcServer> Invoker { get; }
        private IOpenIDKeyStore KeyStore { get; }

        public async Task<IAccessToken> CreateAccessToken(ClaimsIdentity claimsIdentity, CancellationToken cancellationToken)
        {
            var issuer = await GetIssuerAsync(cancellationToken);

            var signingKey = await KeyStore.GetSigningPrivateKey(cancellationToken);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha512Signature);
            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = issuer.ToString(),
                Expires = DateTime.UtcNow.AddYears(1),
                IssuedAt = DateTime.UtcNow,
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials,
                //EncryptingCredentials = encryptingCredentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
            return new JwtAccessToken((JwtSecurityToken)plainToken);
        }

        public async Task<string> GetIssuerAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if(_issuer != null)
            {
                return _issuer;
            }
            else
            {
                var issuer = (await Invoker.GetUriAsync(o => o.OAuth2Discovery(cancellationToken))).ToString();
                issuer = issuer.Substring(0, issuer.IndexOf('/', "https://".Length));
                _issuer = issuer;
                return issuer;
            }
        }

        public Task<IEnumerable<RsaSecurityKey>> GetSigningPublicKeys()
        {
            return KeyStore.GetSigningPublicKeys();
        }
    }
}
