using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.Abstractions.Types.OAuth2;
using SolidRpc.OpenApi.OAuth2.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Invoker
{
    /// <summary>
    /// Tests the invokers
    /// </summary>
    public class HttpInvokerTest1 : WebHostTest
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public HttpInvokerTest1()
        {
            SecKey = Guid.NewGuid();
        }

        /// <summary>
        /// 
        /// </summary>
        public class ComplexStruct
        {
            /// <summary>
            /// 
            /// </summary>
            public int Value { get; set; }
        }


        /// <summary>
        /// 
        /// </summary>
        public interface ITestInterface
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="myStruct"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task<int> DoXAsync(ComplexStruct myStruct, CancellationToken cancellation = default(CancellationToken));

            /// <summary>
            /// 
            /// </summary>
            /// <param name="s"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task<string> DoYAsync(string s, CancellationToken cancellation = default(CancellationToken));

            /// <summary>
            /// Tests the continuation token.
            /// </summary>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task<int> TestContinuationTokenAsync(CancellationToken cancellation = default(CancellationToken));

            /// <summary>
            /// Tests the continuation token.
            /// </summary>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task<FileContent> TestSetCookie(CancellationToken cancellation = default(CancellationToken));

            /// <summary>
            /// Tests proxying a file
            /// </summary>
            /// <param name="fileContent"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task<FileContent> TestProxyFile(FileContent fileContent, CancellationToken cancellation = default(CancellationToken));
        }

        /// <summary>
        /// l
        /// </summary>
        public class TestImplementation : ITestInterface
        {
            public TestImplementation(IInvoker<ITestInterface> invoker)
            {
                Invoker = invoker;
            }

            private IInvoker<ITestInterface> Invoker { get; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="myStruct"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            public Task<int> DoXAsync(ComplexStruct myStruct, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(myStruct.Value);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="s"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            /// <exception cref="NotImplementedException"></exception>
            public Task<string> DoYAsync(string s, CancellationToken cancellation = default)
            {
                return Task.FromResult(s);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            public Task<int> TestContinuationTokenAsync(CancellationToken cancellation = default(CancellationToken))
            {
                ISolidRpcContinuationToken ct = null;
                ct = ct.LoadToken();
                int i = ct.GetToken<int>();
                ct.SetToken(i + 1);
                return Task.FromResult(i);
            }

            public async Task<FileContent> TestSetCookie(CancellationToken cancellation = default)
            {
                var uri = await Invoker.GetUriAsync(o => o.TestSetCookie(cancellation));

                string cookieName = "TestCookie135xyx";
                var invoc = SolidProxy.Core.Proxy.SolidProxyInvocationImplAdvice.CurrentInvocation;
                var cookieValue = ParseCookies(invoc.GetValue<IEnumerable<string>>("http_req_cookie"))
                    .Where(o => o.Name == cookieName)
                    .Select(o => o.Value)
                    .FirstOrDefault();
                cookieValue = cookieValue ?? "0";
                cookieValue = (int.Parse(cookieValue) + 1).ToString();

                var enc = Encoding.UTF8;
                var strContent =  $"Cookie value:{cookieValue}";

                var secure = string.Equals(uri.Scheme, "https", StringComparison.InvariantCultureIgnoreCase) ? "Secure;" : "";
                return new FileContent()
                {
                    CharSet = enc.HeaderName,
                    ContentType = "text/plain",
                    Content = new MemoryStream(enc.GetBytes(strContent)),
                    SetCookie = $"{cookieName}={cookieValue}; Path={uri.AbsolutePath}; Domain={uri.Host}; HttpOnly; SameSite=Strict; {secure}"
                };
            }

            private IEnumerable<NameValuePair> ParseCookies(IEnumerable<string> cookies)
            {
                if (cookies == null) return new NameValuePair[0];
                return cookies.Select(o => {
                    var vals = o.Split(';').First().Split('=');
                    return new NameValuePair() { Name = vals[0], Value = vals[1] };
                });
            }

            public Task<FileContent> TestProxyFile(FileContent fileContent, CancellationToken cancellation)
            {
                if(fileContent.Content.Length == 0)
                {
                    fileContent = null;
                }
                return Task.FromResult(fileContent);
            }
        }

        public class TestOidc : ISolidRpcOidc
        {
            public Task<FileContent> AuthorizeAsync([OpenApi(Name = "scope", In = "query")] IEnumerable<string> scope, [OpenApi(Name = "response_type", In = "query")] string responseType, [OpenApi(Name = "client_id", In = "query")] string clientId, [OpenApi(Name = "redirect_uri", In = "query")] string redirectUri = null, [OpenApi(Name = "state", In = "query")] string state = null, [OpenApi(Name = "response_mode", In = "query")] string responseMode = null, [OpenApi(Name = "nonce", In = "query")] string nonce = null, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(new FileContent()
                {
                    ContentType = "text/plain",
                    Content = new MemoryStream(Encoding.UTF8.GetBytes("Authenticated"))
                });
            }

            public Task<FileContent> EndSessionAsync([OpenApi(Name = "id_token_hint", In = "query")] string idTokenHint, [OpenApi(Name = "post_logout_redirect_uri", In = "query")] string postLogoutRedirectUri, [OpenApi(Name = "state", In = "query")] string state, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<OpenIDConnectDiscovery> GetDiscoveryDocumentAsync(CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<OpenIDKeys> GetKeysAsync(CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<TokenResponse> GetTokenAsync([OpenApi(Name = "grant_type", In = "formData")] string grantType = null, [OpenApi(Name = "client_id", In = "formData")] string clientId = null, [OpenApi(Name = "client_secret", In = "formData")] string clientSecret = null, [OpenApi(Name = "username", In = "formData")] string username = null, [OpenApi(Name = "password", In = "formData")] string password = null, [OpenApi(Name = "scope", In = "formData", CollectionFormat = "ssv")] IEnumerable<string> scope = null, [OpenApi(Name = "code", In = "formData")] string code = null, [OpenApi(Name = "redirect_uri", In = "formData")] string redirectUri = null, [OpenApi(Name = "code_verifier", In = "formData")] string codeVerifier = null, [OpenApi(Name = "refresh_token", In = "formData")] string refreshToken = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task RevokeAsync([OpenApi(Name = "client_id", In = "formData")] string clientId = null, [OpenApi(Name = "client_secret", In = "formData")] string clientSecret = null, [OpenApi(Name = "token", In = "formData")] string token = null, [OpenApi(Name = "token_hint", In = "formData")] string tokenHint = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

        }

        /// <summary>
        /// Configures the server services
        /// </summary>
        /// <param name="services"></param>
        public override void ConfigureServerServices(IServiceCollection services)
        {
            base.ConfigureServerServices(services);
            var openApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            services.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestImplementation), conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SetSecurityKey(SecKey.ToString(), SecKey.ToString());
                return true;
            });
            services.AddSolidRpcOAuth2Local(services.GetSolidRpcService<Uri>().ToString());
            services.AddTransient<ISolidRpcOidc, TestOidc>();
            services.AddSolidRpcServices(o => true);

            services.GetSolidRpcContentStore().AddPrefixRewrite("/test", "/SolidRpc/Tests/Invoker/HttpInvokerTest1/ITestInterface/DoYAsync");
        }

        /// <summary>
        /// Configures the client services
        /// </summary>
        /// <param name="clientServices"></param>
        /// <param name="baseAddress"></param>
        public override void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {
            //
            // test interface
            //
            var openApiSpec = clientServices.GetSolidRpcOpenApiParser()
                .CreateSpecification(typeof(ITestInterface))
                .SetBaseAddress(baseAddress)
                .WriteAsJsonString();
            clientServices.AddSolidRpcBindings(typeof(ITestInterface), null, conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SetSecurityKey(SecKey.ToString(), SecKey.ToString());
                return true;
            });

            //
            // oidc interface
            //
            openApiSpec = clientServices.GetSolidRpcOpenApiParser()
                .CreateSpecification(typeof(ISolidRpcOidc))
                .SetBaseAddress(baseAddress)
                .WriteAsJsonString();
            clientServices.AddSolidRpcRemoteBindings(typeof(ISolidRpcOidc), conf =>
            {
                conf.SetProxyTransportType<IHttpTransport>();
                conf.OpenApiSpec = openApiSpec;
                return true;
            });
            base.ConfigureClientServices(clientServices, baseAddress);
        }

        /// <summary>
        /// The sec key
        /// </summary>
        public Guid SecKey { get; }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestHttpInvokerSimpleInvocation()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var url = await ctx.ClientServiceProvider.GetRequiredService<IInvoker<ITestInterface>>()
                    .GetUriAsync(o => o.DoXAsync(new ComplexStruct() { Value = 4711 }, CancellationToken.None));
                var httpClient = ctx.ClientServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();
                var resp = await httpClient.PostAsync(url, new StringContent("{}"));
                Assert.AreEqual(HttpStatusCode.Unauthorized, resp.StatusCode);

                var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ITestInterface>>();
                var res = await invoker.InvokeAsync(o => o.DoXAsync(new ComplexStruct() { Value = 4711 }, CancellationToken.None), opt => opt);
                Assert.AreEqual(4711, res);
            }
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestHttpInvokerWithPathRewrite()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ITestInterface>>();

                var res = await invoker.InvokeAsync(o => o.DoYAsync("test", CancellationToken.None));
                Assert.AreEqual("test", res);

                var uri = await invoker.GetUriAsync(o => o.DoYAsync("XYZ", CancellationToken.None));
                Assert.IsTrue(uri.AbsolutePath.EndsWith("/XYZ"));

                var httpClient = ctx.ClientServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();
                httpClient.DefaultRequestHeaders.Add(SecKey.ToString(), SecKey.ToString());
                var resp = await httpClient.GetAsync(new Uri(uri, "/test/1/2/3"));
                Assert.IsTrue(resp.IsSuccessStatusCode);
                Assert.AreEqual("\"1/2/3\"", await resp.Content.ReadAsStringAsync());
            }
        }


        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestHttpInvokerCusomOidcImplementation()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var oidc = ctx.ClientServiceProvider.GetRequiredService<ISolidRpcOidc>();
                var res = await oidc.AuthorizeAsync(null, null, null, null, null, null, null, CancellationToken.None);

                Assert.AreEqual("text/plain", res.ContentType);
            }
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestFileContent()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var testInterface = ctx.ClientServiceProvider.GetRequiredService<ITestInterface>();
                var res = await testInterface.TestProxyFile(null);
                Assert.IsNull(res);

                res = await testInterface.TestProxyFile(new FileContent() { 
                    ContentType = "image/jpeg", 
                    Content = new MemoryStream(new byte[10])});
                Assert.AreEqual("image/jpeg", res.ContentType);
                Assert.AreEqual(10, res.Content.Length);

            }
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestHttpInvokerContinuationToken()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ITestInterface>>();
                for (int i = 500; i < 510; i++)
                {
                    var ctReq = Convert.ToBase64String(Encoding.UTF8.GetBytes(i.ToString()));
                    var ctResp = Convert.ToBase64String(Encoding.UTF8.GetBytes((i + 1).ToString()));
                    Assert.AreEqual(i, await invoker.InvokeAsync(o => o.TestContinuationTokenAsync(CancellationToken.None), opts => opts.SetContinuationToken(ctReq).AddPostInvokeCallback(resp =>
                    {
                        var ct = resp.AdditionalHeaders["X-SolidRpc-ContinuationToken"].ToString();
                        Assert.AreEqual(ctResp, ct);
                        return Task.CompletedTask;
                    })));
                }
                //var proxy = ctx.ClientServiceProvider.GetRequiredService<ITestInterface>();
                //for (int i = 0; i < 10; i++)
                //{
                //    Assert.AreEqual(i, await proxy.TestContinuationTokenAsync());
                //}
            }
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestHttpInvokerSetCookie()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ITestInterface>>();

                var cookieContainer = new CookieContainer();
                using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
                using (var client = new HttpClient(handler))
                {
                    var uri = await invoker.GetUriAsync(o => o.TestSetCookie(CancellationToken.None));
                    client.DefaultRequestHeaders.Add(SecKey.ToString(), SecKey.ToString());

                    var req = new HttpRequestMessage(HttpMethod.Get, uri);

                    var resp = await client.SendAsync(req);
                    var strResp = await resp.Content.ReadAsStringAsync();
                    Assert.AreEqual("Cookie value:1", strResp);

                    Assert.AreEqual(1, cookieContainer.Count);

                    resp = await client.GetAsync(uri);
                    strResp = await resp.Content.ReadAsStringAsync();
                    Assert.AreEqual("Cookie value:2", strResp);
                }
            }
        }
    }
}
