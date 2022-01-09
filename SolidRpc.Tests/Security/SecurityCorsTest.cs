using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.OpenApi.Binder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Security
{
    /// <summary>
    /// Tests security functionality.
    /// </summary>
    public class SecurityCorsTest : WebHostTest
    {
        /// <summary>
        /// 
        /// </summary>
        public SecurityCorsTest()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public interface ITestInterface
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            Task TestCorsAsync(CancellationToken cancellationToken = default);
        }

        /// <summary>
        /// 
        /// </summary>
        public class TestImplementation : ITestInterface
        {
            public Task TestCorsAsync(CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientServices"></param>
        /// <param name="baseAddress"></param>
        public override void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {
            var apiSpec = clientServices.GetSolidRpcOpenApiParser()
                .CreateSpecification(typeof(ITestInterface))
                .SetBaseAddress(new Uri(baseAddress, typeof(ITestInterface).Assembly.GetName().Name.Replace('.','/')))
                .WriteAsJsonString();
            clientServices.AddSolidRpcSingletonBindings<ITestInterface>(null, conf =>
            {
                conf.OpenApiSpec = apiSpec;
                return true;
            });
            base.ConfigureClientServices(clientServices, baseAddress);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public override void ConfigureServerServices(IServiceCollection services)
        {
            base.ConfigureServerServices(services);
            var apiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            services.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestImplementation), conf =>
            {
                conf.OpenApiSpec = apiSpec;
                return true;
            });
        }

        /// <summary>
        /// Tests the web host
        /// </summary>
        [Test]
        public async Task TestCors()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var addrTrans = (ConfigurationMethodAddressTransformer)ctx.ServerServiceProvider.GetRequiredService<IMethodAddressTransformer>();
                addrTrans.BaseAddress = new Uri("https://eo-ci-bankid-func.azurewebsites.net/front");
                Assert.IsTrue(addrTrans.AllowedCorsOrigins.Contains("https://eo-ci-bankid-func.azurewebsites.net"));

                addrTrans.ConfiguredCors = new [] { "*" };

                var testInterface = ctx.ClientServiceProvider.GetRequiredService<ITestInterface>();
                await testInterface.TestCorsAsync();

                var invoc = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ITestInterface>>();
                var uri = await invoc.GetUriAsync(o => o.TestCorsAsync(CancellationToken.None));

                var client = ctx.ClientServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();

                addrTrans.ConfiguredCors = new[] { "https://test.test" };
                var resp = await client.SendAsync(new HttpRequestMessage(HttpMethod.Options, uri));
                Assert.AreEqual(HttpStatusCode.NoContent, resp.StatusCode);
                Assert.IsFalse(resp.Headers.TryGetValues("Access-Control-Allow-Origin", out IEnumerable<string> dummy));

                client.DefaultRequestHeaders.Add("origin", "https://localhost");
                resp = await client.SendAsync(new HttpRequestMessage(HttpMethod.Options, uri));
                Assert.AreEqual(HttpStatusCode.Unauthorized, resp.StatusCode);

                addrTrans.ConfiguredCors = new[] { "https://localhost" };
                resp = await client.SendAsync(new HttpRequestMessage(HttpMethod.Options, uri));
                Assert.AreEqual(HttpStatusCode.NoContent, resp.StatusCode);
                Assert.AreEqual("https://localhost", resp.Headers.GetValues("Access-Control-Allow-Origin").First());

            }
        }
    }
}

