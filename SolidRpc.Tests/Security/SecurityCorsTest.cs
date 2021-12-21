﻿using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using System;
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

                var allowedCors = ctx.ServerServiceProvider.GetRequiredService<AllowedCors>();
                allowedCors.Origins = new [] { "*" };

                var testInterface = ctx.ClientServiceProvider.GetRequiredService<ITestInterface>();
                await testInterface.TestCorsAsync();

                var invoc = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ITestInterface>>();
                var uri = await invoc.GetUriAsync(o => o.TestCorsAsync(CancellationToken.None));

                var client = ctx.ClientServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();

                allowedCors.Origins = new[] { "test" };
                var resp = await client.SendAsync(new HttpRequestMessage(HttpMethod.Options, uri));
                Assert.AreEqual(HttpStatusCode.NoContent, resp.StatusCode);

                client.DefaultRequestHeaders.Add("origin", "localhost");
                resp = await client.SendAsync(new HttpRequestMessage(HttpMethod.Options, uri));
                Assert.AreEqual(HttpStatusCode.Unauthorized, resp.StatusCode);

                allowedCors.Origins = new[] { "test", "localhost" };
                resp = await client.SendAsync(new HttpRequestMessage(HttpMethod.Options, uri));
                Assert.AreEqual(HttpStatusCode.NoContent, resp.StatusCode);

            }
        }
    }
}

