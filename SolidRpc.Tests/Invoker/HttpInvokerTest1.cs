using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.Abstractions.Types.OAuth2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
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
            /// Tests the continuation token.
            /// </summary>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task<int> TestContinuationTokenAsync(CancellationToken cancellation = default(CancellationToken));
        }

        /// <summary>
        /// 
        /// </summary>
        public class TestImplementation : ITestInterface
        {
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
    }
}
