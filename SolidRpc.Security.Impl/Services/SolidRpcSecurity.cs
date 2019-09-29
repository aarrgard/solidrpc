using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.Services;
using SolidRpc.Security.Impl.InternalServices;
using SolidRpc.Security.Services;
using SolidRpc.Security.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Security.Impl.Services
{
    public class SolidRpcSecurity : ISolidRpcSecurity
    {
        public SolidRpcSecurity(
            IServiceProvider serviceProvider, 
            IMethodBinderStore methodBinderStore,
            ISolidRpcContentHandler contentHandler,
            IAccessTokenFactory tokenFactory)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            MethodBinderStore = methodBinderStore ?? throw new ArgumentNullException(nameof(methodBinderStore));
            ContentHandler = contentHandler ?? throw new ArgumentNullException(nameof(contentHandler));
            TokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
        }

        private IServiceProvider ServiceProvider { get; }
        private IMethodBinderStore MethodBinderStore { get; }
        private ISolidRpcContentHandler ContentHandler { get; }
        private IAccessTokenFactory TokenFactory { get; }

        public async Task<WebContent> LoginPage(CancellationToken cancellationToken = default(CancellationToken))
        {
            var loginProviders = await LoginProviders(cancellationToken);
            return await LoginProviderBase.GetManifestResourceAsWebContent("SolidRpcSecurity.LoginPage.html", new Dictionary<string, string>()
            {
                {"<!-- login meta-->",string.Join("\r\n", loginProviders.SelectMany(o => o.Meta).Select(o => $"<meta name=\"{o.Name}\" content=\"{o.Content}\"></script>")) },
                {"<!-- login scripts-->", string.Join("\r\n", loginProviders.SelectMany(o => o.Script).Select(o => $"<script src=\"{o}\"></script>")) },
                {"<!-- login buttons-->", string.Join("\r\n", loginProviders.Select(o => o.ButtonHtml)) },
            });
        }

        public async Task<IEnumerable<LoginProvider>> LoginProviders(CancellationToken cancellationToken = default(CancellationToken))
        {
            var loginProviders = ServiceProvider.GetRequiredService<IEnumerable<ILoginProvider>>();
            return await Task.WhenAll(loginProviders.Select(o => o.LoginProvider(cancellationToken)));
        }

        public Task<WebContent> LoginScript(CancellationToken cancellationToken = default(CancellationToken))
        {
            var binder = MethodBinderStore.GetMethodBinding<ISolidRpcSecurity>(o => o.LoginProviders(cancellationToken));
            var auth = binder.MethodBinder.OpenApiSpec.BaseAddress;
            return LoginProviderBase.GetManifestResourceAsWebContent("SolidRpcSecurity.LoginScript.js", new Dictionary<string, string>()
            {
                { "{oidc-client-authority}", auth.ToString() },
                { "{oidc-client-client_id}", ""},
                { "{oidc-client-redirect_uri}", ""},
            });
        }

        public async Task<IEnumerable<Uri>> LoginScripts(CancellationToken cancellationToken = default(CancellationToken))
        {
            var baseScript = await MethodBinderStore.GetUrlAsync<ISolidRpcSecurity>(o => o.LoginScript(cancellationToken));
            var loginProviders = ServiceProvider.GetRequiredService<IEnumerable<ILoginProvider>>();
            var providers = await Task.WhenAll(loginProviders.Select(o => o.LoginProvider(cancellationToken)));
            return providers.SelectMany(o => o.Script).Union(new[] {
                baseScript
            });
        }

        public async Task<OpenIDConnnectDiscovery> OAuth2Discovery(CancellationToken cancellationToken = default(CancellationToken))
        {
            var doc = new OpenIDConnnectDiscovery();
            doc.Issuer = new Uri(await TokenFactory.GetIssuer(cancellationToken));
            doc.JwksUri = await MethodBinderStore.GetUrlAsync<ISolidRpcSecurity>(o => o.OAuth2Keys(cancellationToken));
            doc.TokenEndpoint = await MethodBinderStore.GetUrlAsync<ISolidRpcSecurity>(o => o.OAuth2TokenGet(cancellationToken));
            return doc;
        }

        public async Task<OpenIDKeys> OAuth2Keys(CancellationToken cancellationToken = default(CancellationToken))
        {
            var keys = new OpenIDKeys()
            {
                Keys = (await TokenFactory.GetSigningPublicKeys()).Select(key => {
                    var str = JsonConvert.SerializeObject(JsonWebKeyConverter.ConvertFromSecurityKey(key));
                    return JsonConvert.DeserializeObject<OpenIDKey>(str);
                }).ToList()
            };
            return keys;
        }

        public Task<IEnumerable<Claim>> Profile(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult<IEnumerable<Claim>>(new[] {
                new Claim() {Name = "test", Value = "test"},
                new Claim() {Name = "test2", Value = "test2"},
            });
        }

        public Task<TokenResponse> OAuth2TokenGet(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TokenResponse> OAuth2TokenPost(string grantType = null, string clientId = null, string clientSecret = null, string username = null, string password = null, IEnumerable<string> scope = null, string code = null, string redirectUri = null, string codeVerifier = null, string refreshToken = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            switch (grantType)
            {
                case "client_credentials":
                    return OAuth2TokenClientCredentials(clientId, clientSecret, scope, cancellationToken);
                case "password":
                    return OAuth2TokenPassword(clientId, clientSecret, username, password, scope, cancellationToken);
                case "authorization_code":
                    return OAuth2TokenAuthorizationCode(clientId, clientSecret, code, redirectUri, codeVerifier, cancellationToken);
                case "refresh_token":
                    return OAuth2TokenRefreshToken(clientId, clientSecret, refreshToken, cancellationToken);
                default:
                    throw new Exception("Invalid grant type:"+grantType);
            }
        }

        private async Task<TokenResponse> OAuth2TokenRefreshToken(string clientId, string clientSecret, string refreshToken, CancellationToken cancellationToken)
        {
            var claimsIdentity = new System.Security.Claims.ClaimsIdentity();
            claimsIdentity.AddClaim(new System.Security.Claims.Claim("client_id", clientId));
            var accessToken = await TokenFactory.CreateAccessToken(claimsIdentity, cancellationToken);
            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
                ExpiresIn = accessToken.ExpiresInSeconds.ToString(),
                TokenType = accessToken.TokenType
            };
        }

        private async Task<TokenResponse> OAuth2TokenAuthorizationCode(string clientId, string clientSecret, string code, string redirectUri, string codeVerifier, CancellationToken cancellationToken)
        {
            var claimsIdentity = new System.Security.Claims.ClaimsIdentity();
            claimsIdentity.AddClaim(new System.Security.Claims.Claim("client_id", clientId));
            var accessToken = await TokenFactory.CreateAccessToken(claimsIdentity, cancellationToken);
            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
                ExpiresIn = accessToken.ExpiresInSeconds.ToString(),
                TokenType = accessToken.TokenType
            };
        }

        private async Task<TokenResponse> OAuth2TokenPassword(string clientId, string clientSecret, string username, string password, IEnumerable<string> scope, CancellationToken cancellationToken)
        {
            var claimsIdentity = new System.Security.Claims.ClaimsIdentity();
            claimsIdentity.AddClaim(new System.Security.Claims.Claim("client_id", clientId));
            claimsIdentity.AddClaim(new System.Security.Claims.Claim("sub", username));
            var accessToken = await TokenFactory.CreateAccessToken(claimsIdentity, cancellationToken);
            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
                ExpiresIn = accessToken.ExpiresInSeconds.ToString(),
                Scope = scope,
                TokenType = accessToken.TokenType
            };
        }

        private async Task<TokenResponse> OAuth2TokenClientCredentials(string clientId, string clientSecret, IEnumerable<string> scope, CancellationToken cancellationToken)
        {
            var claimsIdentity = new System.Security.Claims.ClaimsIdentity();
            claimsIdentity.AddClaim(new System.Security.Claims.Claim("client_id", clientId));
            var accessToken = await TokenFactory.CreateAccessToken(claimsIdentity, cancellationToken);
            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
                ExpiresIn = accessToken.ExpiresInSeconds.ToString(),
                Scope = scope,
                TokenType = accessToken.TokenType
            };
        }

        public Task<WebContent> OAuth2AuthorizeGet(IEnumerable<string> scope, string responseType, string clientId, string redirectUri = null, string state = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new WebContent()
            {
                Content = new MemoryStream(Encoding.UTF8.GetBytes("Please authenticate!")),
                ContentType = "text/plain",
                CharSet = Encoding.UTF8.EncodingName
            });
        }

        public Task<WebContent> OAuth2AuthorizePost(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new WebContent()
            {
                Content = new MemoryStream(Encoding.UTF8.GetBytes("Please authenticate!")),
                ContentType = "text/plain",
                CharSet = Encoding.UTF8.EncodingName
            });
        }
    }
}
