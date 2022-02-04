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
using System.Security.Claims;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.OpenApi.Invoker;
using System.Net;
using System.Text.RegularExpressions;
using SolidRpc.Abstractions.InternalServices;
using System.Threading;

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
            /// <param name="arg"></param>
            /// <returns></returns>
            Task<string> GetProtectedResource(string arg);

            /// <summary>
            /// A test method
            /// </summary>
            /// <param name="arg"></param>
            /// <returns></returns>
            Task<string> GetProtectedResourceWithRedirect(string arg, string optional = null);
        }

        /// <summary>
        /// A test implementation
        /// </summary>
        public class OAuth2ProtectedService : IOAuth2ProtectedService
        {
            /// <summary>
            /// Constructs a new instance
            /// </summary>
            /// <param name="principal"></param>
            public OAuth2ProtectedService(ClaimsPrincipal principal)
            {
                Principal = principal;
            }

            public IPrincipal Principal { get; }

            /// <summary>
            /// A test method
            /// </summary>
            /// <param name="arg"></param>
            /// <returns></returns>
            public Task<string> GetProtectedResource(string arg)
            {
                return Task.FromResult(arg + ":" + Principal.Identity.Name);
            }


            /// <summary>
            /// A test method
            /// </summary>
            /// <param name="arg"></param>
            /// <returns></returns>
            public Task<string> GetProtectedResourceWithRedirect(string arg, string optional = null)
            {
                return Task.FromResult(arg + ":" + Principal.Identity.Name);
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
            clientServices.AddSolidRpcBindings(typeof(ISolidRpcOAuth2), null, o => ConfigureClientService(clientServices, o, baseAddress));
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
                conf.SetOAuth2ClientSecurity(GetIssuer(baseAddress), "clientid", "secret");
                conf.DisableSecurity();
            }
            if (conf.Methods.Single().Name == nameof(IOAuth2EnabledService.GetUserEnabledResource))
            {
                conf.SetOAuth2ProxySecurity();
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
            serverServices.AddSolidRpcOAuth2Local(GetIssuer(serverServices.GetSolidRpcService<Uri>()).ToString(), o => o.CreateSigningKey());
            serverServices.AddSolidRpcServices();
            var openApi = serverServices.GetSolidRpcOpenApiParser().CreateSpecification(typeof(IOAuth2EnabledService).GetMethods().Union(typeof(IOAuth2ProtectedService).GetMethods()).ToArray()).WriteAsJsonString();
            serverServices.AddSolidRpcBindings(typeof(IOAuth2EnabledService), typeof(OAuth2EnabledService), o =>
            {
                o.OpenApiSpec = openApi;
                o.GetAdviceConfig<ISecurityOAuth2Config>().OAuth2Authority = GetIssuer(serverServices.GetSolidRpcService<Uri>());
                return true;
            });
            serverServices.AddSolidRpcBindings(typeof(IOAuth2ProtectedService), typeof(OAuth2ProtectedService), o =>
            {
                o.OpenApiSpec = openApi;
                var oauth2Config = o.SetOAuth2ClientSecurity(GetIssuer(serverServices.GetSolidRpcService<Uri>()), "clientid", "secret");
                if (o.Methods.Single().Name == nameof(IOAuth2ProtectedService.GetProtectedResourceWithRedirect))
                {
                    oauth2Config.RedirectUnauthorizedIdentity = true;
                }
                o.GetAdviceConfig<ISecurityPathClaimConfig>().Enabled = true;
                return true;
            });


            serverServices.AddSolidRpcServices(c =>
            {
                if(c.Methods.Single().DeclaringType == typeof(ISolidRpcOAuth2))
                {
                    c.SetOAuth2ClientSecurity(GetIssuer(serverServices.GetSolidRpcService<Uri>()), "clientid", "secret");
                    c.DisableSecurity();
                }
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
                var res = await httpClient.GetDiscoveryDocumentAsync(GetIssuer(ctx.BaseAddress));
                Assert.IsFalse(res.IsError);

                Assert.AreEqual(GetIssuer(ctx.BaseAddress), res.Issuer);
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
                var authority = authFactory.GetAuthority(GetIssuer(ctx.BaseAddress));
                var doc = await authority.GetDiscoveryDocumentAsync();
                var keys = await authority.GetSigningKeysAsync();

                Assert.AreEqual(GetIssuer(ctx.BaseAddress), doc.Issuer);
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
                var res = await httpClient.GetDiscoveryDocumentAsync(GetIssuer(ctx.BaseAddress));

                // authenticate client
                var response = await httpClient.RequestTokenAsync(new TokenRequest
                {
                    Address = res.TokenEndpoint,
                    GrantType = "client_credentials",

                    ClientCredentialStyle = ClientCredentialStyle.PostBody,
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

        private string GetIssuer(Uri baseAddress)
        {
            return new Uri(baseAddress, "SolidRpc/Abstractions").ToString();
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
                var res = await httpClient.GetDiscoveryDocumentAsync(GetIssuer(ctx.BaseAddress));

                // authenticate client
                var response = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
                {
                    Address = res.TokenEndpoint,

                    ClientCredentialStyle = ClientCredentialStyle.PostBody,
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
                var res = await httpClient.GetDiscoveryDocumentAsync(GetIssuer(ctx.BaseAddress));

                // authenticate client
                var response = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = res.TokenEndpoint,

                    ClientCredentialStyle = ClientCredentialStyle.PostBody,
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
                var res = await httpClient.GetDiscoveryDocumentAsync(GetIssuer(ctx.BaseAddress));

                var authResp = await httpClient.GetAsync($"{res.AuthorizeEndpoint}?redirect_uri=http://test.test/test&client_id=y&response_type=code");
                Assert.AreEqual(HttpStatusCode.Found, authResp.StatusCode);
                var code = authResp.Headers.Location.Query.Split('?')[1].Split('&').Where(o => o.StartsWith("code=")).Single().Split('=')[1];

                // authenticate client
                var response = await httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
                {
                    Address = res.TokenEndpoint,

                    ClientId = "client",
                    ClientSecret = "secret",

                    Code = code,
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
                var res = await httpClient.GetDiscoveryDocumentAsync(GetIssuer(ctx.BaseAddress));

                // authenticate user
                var response = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = res.TokenEndpoint,

                    ClientCredentialStyle = ClientCredentialStyle.PostBody,
                    ClientId = "client",
                    ClientSecret = "secret",
                    Scope = "api1",

                    UserName = "bob",
                    Password = "bob"
                });

                ValidateAccessToken(res, response);

                // use refresh token to get new access token
                response = await httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = res.TokenEndpoint,

                    ClientId = "client",
                    ClientSecret = "secret",

                    RefreshToken = response.RefreshToken
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

                //
                // Test invoking using the configured client
                //
                var res = await protectedService.GetClientEnabledResource("test");
                Assert.AreEqual("test:clientid", res);

                var authLocal = ctx.ClientServiceProvider.GetRequiredService<IAuthorityFactory>().GetAuthority(GetIssuer(ctx.BaseAddress));

                //
                // Test user as user principal
                //
                var userJwt = await authLocal.GetUserJwtAsync("clientid", "clientsecret", "userid", "password", new[] { "scope1", "scope2" });
                var prin = await authLocal.GetPrincipalAsync(userJwt.AccessToken);
                Assert.IsNotNull(prin.Claims.Where(o => o.Type == "scope").Where(o => o.Value == "scope1").Single());
                Assert.IsNotNull(prin.Claims.Where(o => o.Type == "scope").Where(o => o.Value == "scope2").Single());
                ctx.ClientServiceProvider.GetRequiredService<ISolidRpcAuthorization>().CurrentPrincipal = prin;
                res = await protectedService.GetUserEnabledResource("test");
                Assert.AreEqual("test:userid", res);

                //
                // Test client as user principal
                //
                var clientJwt1 = await authLocal.GetClientJwtAsync("clientid", "clientsecret", new[] { "test" });
                var clientJwt2 = await authLocal.GetClientJwtAsync("clientid", "clientsecret", new[] { "test" });
                Assert.AreEqual(clientJwt1.AccessToken, clientJwt2.AccessToken);
                ctx.ClientServiceProvider.GetRequiredService<ISolidRpcAuthorization>().CurrentPrincipal = await authLocal.GetPrincipalAsync(clientJwt1.AccessToken);
                res = await protectedService.GetUserEnabledResource("test");
                Assert.AreEqual("test:clientid", res);
                
                //
                // Test resetting the current principal
                //
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
                    var res = await protectedService.GetProtectedResource("test");
                    Assert.Fail();
                }
                catch(UnauthorizedException)
                {
                    // This is the way we want it..
                }

                // invoke directly - intercept 302
                var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<IOAuth2ProtectedService>>();
                var arg1 = Guid.NewGuid().ToString();
                var arg2 = Guid.NewGuid().ToString();
                var arg3 = Guid.NewGuid().ToString();
                var uri = (await invoker.GetUriAsync(o => o.GetProtectedResourceWithRedirect(arg1, arg2))) + $"&additional_query={arg3}";
                var httpClient = ctx.ClientServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();
                var resp = await httpClient.GetAsync(uri);
                Assert.AreEqual(HttpStatusCode.Found, resp.StatusCode);

                // fetch token
                resp = await httpClient.GetAsync(resp.Headers.Location);
                Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);

                // continue on to the OAuth server
                var content = await resp.Content.ReadAsStringAsync();
                var re = new Regex("'([^']+&scope=)'");
                var strUri = re.Match(content).Groups[1].Value;
                resp = await httpClient.GetAsync(new Uri(strUri));

                Assert.AreEqual(HttpStatusCode.Found, resp.StatusCode);
                resp = await httpClient.GetAsync(resp.Headers.Location);
                Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);

                // now go back to the original page
                content = await resp.Content.ReadAsStringAsync();
                re = new Regex("accessToken = '([^']+)';");
                var accessToken = re.Match(content).Groups[1].Value;
                re = new Regex("callback = '([^']+)';");
                var callback = re.Match(content).Groups[1].Value;
                Assert.IsTrue(callback.Contains(arg1));
                Assert.IsTrue(callback.Contains(arg2));
                Assert.IsTrue(callback.Contains(arg3));

                resp = await httpClient.GetAsync(new Uri($"{callback}"));
                Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);

                ValidateAccessToken(ctx.ServerServiceProvider.GetRequiredService<IAuthorityLocal>(), accessToken);

                //
                // lets refresh the access token
                //
                await Task.Delay(1000); // we want a new token - make sure that the expiry is not same
                var oauthInvoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ISolidRpcOAuth2>>();
                var refreshUri = await oauthInvoker.GetUriAsync(o => o.RefreshTokenAsync(accessToken, CancellationToken.None));
                resp = await httpClient.GetAsync(refreshUri);
                Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
                content = await resp.Content.ReadAsStringAsync();

                ValidateAccessToken(ctx.ServerServiceProvider.GetRequiredService<IAuthorityLocal>(), content);

                Assert.AreNotEqual(accessToken, content);

                // get current access token
                refreshUri = await oauthInvoker.GetUriAsync(o => o.RefreshTokenAsync("current", CancellationToken.None));
                Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
                var currentContent = await resp.Content.ReadAsStringAsync();

                ValidateAccessToken(ctx.ServerServiceProvider.GetRequiredService<IAuthorityLocal>(), currentContent);
            }
        }

        private void ValidateAccessToken(IAuthorityLocal authorityLocal, string accessToken)
        {
            var tokenValidationParameter = new TokenValidationParameters()
            {
                ValidateActor = false,
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidIssuer = authorityLocal.Authority,
                ValidateIssuerSigningKey = false,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                RequireSignedTokens = false,
                IssuerSigningKeyResolver = (token, st, kid, validationParameters) => {
                    var authKeys = authorityLocal.GetSigningKeysAsync().Result.ToList();
                    var keys = JsonConvert.SerializeObject(new SolidRpc.Abstractions.Types.OAuth2.OpenIDKeys() { Keys = authKeys });
                    var signingKeys = Microsoft.IdentityModel.Tokens.JsonWebKeySet.Create(keys)
                    .GetSigningKeys().Where(o => o.KeyId == kid).ToList();
                    return signingKeys;
                }

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var claimsPrincipal = tokenHandler.ValidateToken(accessToken, tokenValidationParameter, out securityToken);
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
