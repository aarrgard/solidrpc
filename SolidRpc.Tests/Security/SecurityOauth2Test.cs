using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using IdentityModel.Client;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using SolidRpc.Abstractions.OpenApi.Proxy;
using System.Security.Principal;
using SolidRpc.Abstractions.Types;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using IdentityModel;
using System.Security.Claims;
using SolidRpc.Abstractions.Services;

namespace SolidRpc.Tests.Security
{
    /// <summary>
    /// Tests security functionality.
    /// </summary>
    public class SecurityOauth2Test : WebHostTest
    {
        /// <summary>
        /// A test interface
        /// </summary>
        public interface IOAuth2EnabledService
        {
            /// <summary>
            /// A test method
            /// </summary>
            /// <param name="arg"></param>
            /// <returns></returns>
            Task<string> GetClientEnabledResource(string arg);
            
            /// <summary>
            /// A test method
            /// </summary>
            /// <param name="arg"></param>
            /// <returns></returns>
            Task<string> GetUserEnabledResource(string arg);
        }

        /// <summary>
        /// A test implementation
        /// </summary>
        public class OAuth2EnabledService : IOAuth2EnabledService
        {
            /// <summary>
            /// Constructs a new instance
            /// </summary>
            /// <param name="principal"></param>
            public OAuth2EnabledService(ClaimsPrincipal principal)
            {
                Principal = principal;
            }

            public IPrincipal Principal { get; }

            /// <summary>
            /// A test method
            /// </summary>
            /// <param name="arg"></param>
            /// <returns></returns>
            public Task<string> GetClientEnabledResource(string arg)
            {
                return Task.FromResult(arg + ":" + Principal.Identity.Name);
            }

            /// <summary>
            /// A test method
            /// </summary>
            /// <param name="arg"></param>
            /// <returns></returns>
            public Task<string> GetUserEnabledResource(string arg)
            {
                return Task.FromResult(arg + ":" + Principal.Identity.Name);
            }
        }
        /// <summary>
        /// A test interface
        /// </summary>
        public interface IOAuth2ProtectedService
        {
            /// <summary>
            /// A test method
            /// </summary>
            /// <param name="principal"></param>
            /// <param name="arg"></param>
            /// <returns></returns>
            Task<string> GetProtectedResource(IPrincipal principal, string arg);
        }

        /// <summary>
        /// A test implementation
        /// </summary>
        public class OAuth2ProtectedService : IOAuth2ProtectedService
        {
            /// <summary>
            /// A test method
            /// </summary>
            /// <param name="principal"></param>
            /// <param name="arg"></param>
            /// <returns></returns>
            public Task<string> GetProtectedResource(IPrincipal principal, string arg)
            {
                return Task.FromResult(arg + ":" + principal.Identity.Name);
            }
        }

        /// <summary>
        /// Configures the client services
        /// </summary>
        /// <param name="clientServices"></param>
        /// <param name="baseAddress"></param>
        public override void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {
            clientServices.AddHttpClient();
            clientServices.AddSolidRpcOAuth2();
            clientServices.AddSolidRpcBindings(typeof(IOAuth2EnabledService), null, o => ConfigureClientService(clientServices, o, baseAddress));
            clientServices.AddSolidRpcBindings(typeof(IOAuth2ProtectedService), null, o => ConfigureClientService(clientServices, o, baseAddress));
            base.ConfigureClientServices(clientServices, baseAddress);
        }

        private bool ConfigureClientService(IServiceCollection clientServices, ISolidRpcOpenApiConfig conf, Uri baseAddress)
        {

            conf.OpenApiSpec = clientServices.GetSolidRpcOpenApiParser()
                .CreateSpecification(conf.Methods.ToArray())
                .SetBaseAddress(baseAddress)
                .WriteAsJsonString();

            if (conf.Methods.Single().Name == nameof(IOAuth2EnabledService.GetClientEnabledResource))
            {
                var oauth2Config = conf.GetAdviceConfig<ISecurityOAuth2Config>();
                oauth2Config.OAuth2Authority = baseAddress.ToString();
                oauth2Config.OAuth2ClientId = "clientid";
                oauth2Config.OAuth2ClientSecret = "secret";
                oauth2Config.OAuthProxyInvocationPrincipal = OAuthProxyInvocationPrincipal.Client;
            }
            if (conf.Methods.Single().Name == nameof(IOAuth2EnabledService.GetUserEnabledResource))
            {
                var oauth2Config = conf.GetAdviceConfig<ISecurityOAuth2Config>();
                oauth2Config.OAuthProxyInvocationPrincipal = OAuthProxyInvocationPrincipal.Proxy;
            }


            return true;
        }

        /// <summary>
        /// Configures the server services
        /// </summary>
        /// <param name="serverServices"></param>
        /// <returns></returns>
        public override void ConfigureServerServices(IServiceCollection serverServices)
        {
            base.ConfigureServerServices(serverServices);
            serverServices.AddSolidRpcOAuth2Local(serverServices.GetSolidRpcService<Uri>().ToString(), o => o.CreateSigningKey());
            serverServices.AddSolidRpcSecurityBackend();
            var openApi = serverServices.GetSolidRpcOpenApiParser().CreateSpecification(typeof(IOAuth2EnabledService).GetMethods().Union(typeof(IOAuth2ProtectedService).GetMethods()).ToArray()).WriteAsJsonString();
            serverServices.AddSolidRpcBindings(typeof(IOAuth2EnabledService), typeof(OAuth2EnabledService), o =>
            {
                o.OpenApiSpec = openApi;
                o.GetAdviceConfig<ISecurityOAuth2Config>().OAuth2Authority = serverServices.GetSolidRpcService<Uri>().ToString();
                return true;
            });
            serverServices.AddSolidRpcBindings(typeof(IOAuth2ProtectedService), typeof(OAuth2ProtectedService), o =>
            {
                o.OpenApiSpec = openApi;
                o.GetAdviceConfig<ISecurityOAuth2Config>().OAuth2Authority = serverServices.GetSolidRpcService<Uri>().ToString();
                o.GetAdviceConfig<ISecurityPathClaimConfig>().Enabled = true;
                return true;
            });
        }

        /// <summary>
        /// Tests the web host
        /// </summary>
        [Test]
        public async Task TestOauthDiscoveryUsingHttpClient()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var clientFactory = ctx.ClientServiceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = clientFactory.CreateClient();
                var res = await httpClient.GetDiscoveryDocumentAsync(ctx.BaseAddress.ToString());
                Assert.IsFalse(res.IsError);

                var issuer = ctx.BaseAddress.ToString();
                Assert.AreEqual(issuer, res.Issuer);
                Assert.AreEqual(1, res.KeySet.Keys.Count);
            }
        }

        /// <summary>
        /// Tests the web host
        /// </summary>
        [Test]
        public async Task TestOauthDiscoveryUsingAuthFactory()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var authFactory = ctx.ClientServiceProvider.GetRequiredService<IAuthorityFactory>();
                var authority = authFactory.GetAuthority(ctx.BaseAddress.ToString());
                var doc = await authority.GetDiscoveryDocumentAsync();
                var keys = await authority.GetSigningKeysAsync();

                Assert.AreEqual(ctx.BaseAddress, doc.Issuer);
                Assert.AreEqual(1, keys.Count());
            }
        }

        /// <summary>
        /// Tests the web host
        /// </summary>
        [Test]
        public async Task TestOauthTokenRequest()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var clientFactory = ctx.ClientServiceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = clientFactory.CreateClient();
                var res = await httpClient.GetDiscoveryDocumentAsync(ctx.BaseAddress.ToString());


                // authenticate client
                var response = await httpClient.RequestTokenAsync(new TokenRequest
                {
                    Address = res.TokenEndpoint,
                    GrantType = "client_credentials",

                    ClientId = "client",
                    ClientSecret = "secret",

                    Parameters =
                    {
                        { "custom_parameter", "custom value"},
                        { "scope", "api1 api2" }
                    }
                });

                ValidateAccessToken(res, response);

            }
        }

        /// <summary>
        /// Tests the web host
        /// </summary>
        [Test]
        public async Task TestOauthClientCredentialsToken()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var clientFactory = ctx.ClientServiceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = clientFactory.CreateClient();
                var res = await httpClient.GetDiscoveryDocumentAsync(ctx.BaseAddress.ToString());

                // authenticate client
                var response = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = res.TokenEndpoint,

                    ClientId = "client",
                    ClientSecret = "secret",
                    Scope = "api1"
                });

                ValidateAccessToken(res, response);
            }
        }

        /// <summary>
        /// Tests the web host
        /// </summary>
        [Test]
        public async Task TestOauthPasswordToken()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var clientFactory = ctx.ClientServiceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = clientFactory.CreateClient();
                var res = await httpClient.GetDiscoveryDocumentAsync(ctx.BaseAddress.ToString());

                // authenticate client
                var response = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = res.TokenEndpoint,

                    ClientId = "client",
                    ClientSecret = "secret",
                    Scope = "api1",

                    UserName = "bob",
                    Password = "bob"
                });

                ValidateAccessToken(res, response);
            }
        }

        /// <summary>
        /// Tests the web host
        /// </summary>
        [Test]
        public async Task TestOauthAuthorizationCodeToken()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var clientFactory = ctx.ClientServiceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = clientFactory.CreateClient();
                var res = await httpClient.GetDiscoveryDocumentAsync(ctx.BaseAddress.ToString());

                // authenticate client
                var response = await httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
                {
                    Address = res.TokenEndpoint,

                    ClientId = "client",
                    ClientSecret = "secret",

                    Code = "my code",
                    RedirectUri = "https://app.com/callback",

                    // optional PKCE parameter
                    CodeVerifier = "xyz"
                });

                ValidateAccessToken(res, response);
            }
        }

        /// <summary>
        /// Tests the web host
        /// </summary>
        [Test]
        public async Task TestOauthRefreshToken()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var clientFactory = ctx.ClientServiceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = clientFactory.CreateClient();
                var res = await httpClient.GetDiscoveryDocumentAsync(ctx.BaseAddress.ToString());

                // authenticate client
                var response = await httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = res.TokenEndpoint,

                    ClientId = "client",
                    ClientSecret = "secret",

                    RefreshToken = "xyz"
                });

                ValidateAccessToken(res, response);
            }
        }

        /// <summary>
        /// Tests the web host
        /// </summary>
        [Test]
        public async Task TestOauthEnabledResource()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var protectedService = ctx.ClientServiceProvider.GetRequiredService<IOAuth2EnabledService>();

                var res = await protectedService.GetClientEnabledResource("test");
                Assert.AreEqual("test:clientid", res);

                var authLocal = ctx.ClientServiceProvider.GetRequiredService<IAuthorityFactory>().GetAuthority(ctx.BaseAddress.ToString());
                var userJwt = await authLocal.GetUserJwtAsync("clientid", "clientsecret", "userid", "password", new[] { "test" });

                ctx.ClientServiceProvider.GetRequiredService<ISolidRpcAuthorization>().CurrentPrincipal = await authLocal.GetPrincipalAsync(userJwt);
                res = await protectedService.GetUserEnabledResource("test");
                Assert.AreEqual("test:userid", res);

                var clientJwt = await authLocal.GetClientJwtAsync("clientid", "clientsecret", new[] { "test" });

                ctx.ClientServiceProvider.GetRequiredService<ISolidRpcAuthorization>().CurrentPrincipal = await authLocal.GetPrincipalAsync(clientJwt);
                res = await protectedService.GetUserEnabledResource("test");
                Assert.AreEqual("test:clientid", res);
                
                try
                {
                    ctx.ClientServiceProvider.GetRequiredService<ISolidRpcAuthorization>().CurrentPrincipal = null;
                    res = await protectedService.GetUserEnabledResource("test");
                    Assert.Fail();
                } catch(UnauthorizedException)
                {
                    // this is the way we want it
                }
            }
        }

        /// <summary>
        /// Tests the web host
        /// </summary>
        [Test]
        public async Task TestOauthProtectedResource()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var protectedService = ctx.ClientServiceProvider.GetRequiredService<IOAuth2ProtectedService>();
                try
                {
                    var res = await protectedService.GetProtectedResource(null, "test");
                    Assert.Fail();
                }
                catch(UnauthorizedException)
                {
                    // This is the way we want it..
                }
            }
        }

        private void ValidateAccessToken(DiscoveryDocumentResponse discResp, TokenResponse resp)
        {
            Assert.IsFalse(discResp.IsError);
            Assert.IsFalse(resp.IsError);
            var tokenValidationParameter = new TokenValidationParameters()
            {
                //ValidateTokenReplay = false,
                ValidateActor = false,
                ValidateAudience = false,
                ValidIssuer = discResp.Issuer,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                IssuerSigningKeyResolver = (token, st, kid, validationParameters) => {
                    var keys = JsonConvert.SerializeObject(discResp.KeySet);
                    var signingKeys = Microsoft.IdentityModel.Tokens.JsonWebKeySet.Create(keys)
                    .GetSigningKeys().Where(o => o.KeyId == kid).ToList();
                    return signingKeys;
                }
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var claimsPrincipal = tokenHandler.ValidateToken(resp.AccessToken, tokenValidationParameter, out securityToken);
        }
    }
}
