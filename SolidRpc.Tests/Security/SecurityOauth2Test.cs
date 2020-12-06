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
using SolidRpc.OpenApi.OAuth2.InternalServices;
using SolidRpc.Abstractions.OpenApi.Proxy;

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
        public interface IOAuthProtectedService
        {
            /// <summary>
            /// A test method
            /// </summary>
            /// <param name="arg"></param>
            /// <returns></returns>
            Task<string> InvokeAsync(string arg);
        }

        /// <summary>
        /// A test implementation
        /// </summary>
        public class OAuthProtectedService : IOAuthProtectedService
        {
            /// <summary>
            /// A test method
            /// </summary>
            /// <param name="arg"></param>
            /// <returns></returns>
            public Task<string> InvokeAsync(string arg)
            {
                return Task.FromResult(arg);
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
            clientServices.AddSolidRpcBindings(typeof(IOAuthProtectedService), null, o =>
            {

                o.OpenApiSpec = clientServices.GetSolidRpcOpenApiParser().CreateSpecification(o.Methods.ToArray())
                    .SetBaseAddress(baseAddress)
                    .WriteAsJsonString();

                var oauth2Config = o.GetAdviceConfig<ISecurityOAuth2Config>();
                oauth2Config.OAuth2Authority = baseAddress;
                oauth2Config.OAuth2ClientId = "clientid";
                oauth2Config.OAuth2ClientSecret = "secret";
                oauth2Config.OAuthProxyInvocationPrincipal = OAuthProxyInvocationPrincipal.Client;

                return true;
            });
            base.ConfigureClientServices(clientServices, baseAddress);
        }
        /// <summary>
        /// Configures the server services
        /// </summary>
        /// <param name="serverServices"></param>
        /// <returns></returns>
        public override void ConfigureServerServices(IServiceCollection serverServices)
        {
            base.ConfigureServerServices(serverServices);
            serverServices.AddSolidRpcOAuth2();
            serverServices.AddSolidRpcSecurityFrontend();
            serverServices.AddSolidRpcSecurityBackend();
            serverServices.AddSolidRpcBindings(typeof(IOAuthProtectedService), typeof(OAuthProtectedService), o =>
            {

                o.OpenApiSpec = serverServices.GetSolidRpcOpenApiParser().CreateSpecification(o.Methods.ToArray()).WriteAsJsonString();
                o.GetAdviceConfig<ISecurityOAuth2Config>().OAuth2Authority = new Uri("http://localhost:5000/");
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
                Assert.AreEqual(issuer.Substring(0, issuer.Length - 1), res.Issuer);
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
                var authority = authFactory.GetAuthority(ctx.BaseAddress);
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
        public async Task TestOauthProtectedResource()
        {
            using (var ctx = CreateKestrelHostContext(serverServices =>
            {
                serverServices.AddSolidRpcApplicationInsights(OpenApi.ApplicationInsights.LogSettings.ErrorScopes, "RequestId");
            }))
            {
                await ctx.StartAsync();
                var protectedService = ctx.ClientServiceProvider.GetRequiredService<IOAuthProtectedService>();

                var res = await protectedService.InvokeAsync("test");
                Assert.AreEqual("test", res);
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
