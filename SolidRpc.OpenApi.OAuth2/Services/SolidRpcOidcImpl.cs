using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.Abstractions.Types.OAuth2;
using SolidRpc.OpenApi.OAuth2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(ISolidRpcOidc), typeof(SolidRpcOidcImpl), SolidRpcServiceLifetime.Scoped)]
namespace SolidRpc.OpenApi.OAuth2.Services
{
    /// <summary>
    /// Implements an Oidc server
    /// </summary>
    public class SolidRpcOidcImpl : ISolidRpcOidc
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="localAuthority"></param>
        public SolidRpcOidcImpl(
            IInvoker<ISolidRpcOidc> invoker,
            IAuthorityLocal localAuthority = null)
        {
            Invoker = invoker;
            LocalAuthority = localAuthority;
        }

        protected IInvoker<ISolidRpcOidc> Invoker { get; }
        protected IAuthorityLocal LocalAuthority { get; }

        /// <summary>
        /// Returns the discovery document
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<OpenIDConnectDiscovery> GetDiscoveryDocumentAsync(CancellationToken cancellationToken = default)
        {
            var jwksUri = await Invoker.GetUriAsync(o => o.GetKeysAsync(cancellationToken), false);
            return new OpenIDConnectDiscovery()
            {
                Issuer = LocalAuthority.Authority,
                JwksUri = jwksUri,
            };
        }

        /// <summary>
        /// Get keys
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<OpenIDKeys> GetKeysAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<OpenIDKey> keys;
            if (LocalAuthority == null)
            {
                keys = new OpenIDKey[0];
            }
            else
            {
                keys = await LocalAuthority.GetSigningKeysAsync(cancellationToken);
            }
            return new OpenIDKeys() { Keys = keys.ToArray() };
        }

        /// <summary>
        /// Authorizes a client or user
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="responseType"></param>
        /// <param name="clientId"></param>
        /// <param name="redirectUri"></param>
        /// <param name="state"></param>
        /// <param name="responseMode"></param>
        /// <param name="nonce"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<FileContent> AuthorizeAsync(
            IEnumerable<string> scope,
            string responseType, 
            string clientId, 
            string redirectUri = null, 
            string state = null, 
            string responseMode = null, 
            string nonce = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// REturns the token
        /// </summary>
        /// <param name="grantType"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="scope"></param>
        /// <param name="code"></param>
        /// <param name="redirectUri"></param>
        /// <param name="codeVerifier"></param>
        /// <param name="refreshToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<TokenResponse> GetTokenAsync(string grantType = null, string clientId = null, string clientSecret = null, string username = null, string password = null, IEnumerable<string> scope = null, string code = null, string redirectUri = null, string codeVerifier = null, string refreshToken = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="token"></param>
        /// <param name="tokenHint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual Task RevokeAsync(string clientId = null,  string clientSecret = null,  string token = null,  string tokenHint = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
