using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Services;
using System;
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

        }

        /// <summary>
        /// Configures the client services
        /// </summary>
        /// <param name="clientServices"></param>
        /// <param name="baseAddress"></param>
        public override void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {
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
        public async Task TestHttpInvokerContinuationToken()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                //var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ITestInterface>>();
                //for (int i = 500; i < 510; i++)
                //{
                //    var ctReq = Convert.ToBase64String(Encoding.UTF8.GetBytes(i.ToString()));
                //    var ctResp = Convert.ToBase64String(Encoding.UTF8.GetBytes((i + 1).ToString()));
                //    Assert.AreEqual(i, await invoker.InvokeAsync(o => o.TestContinuationTokenAsync(CancellationToken.None), opts => opts.SetContinuationToken(ctReq).AddPostInvokeCallback(resp =>
                //    {
                //        var ct = resp.AdditionalHeaders["X-SolidRpc-ContinuationToken"].ToString();
                //        Assert.AreEqual(ctResp, ct);
                //        return Task.CompletedTask;
                //    })));
                //}
                var proxy = ctx.ClientServiceProvider.GetRequiredService<ITestInterface>();
                for (int i = 0; i < 10; i++)
                {
                    Assert.AreEqual(i, await proxy.TestContinuationTokenAsync());
                }
            }
        }
    }
}
