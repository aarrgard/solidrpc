﻿using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
using System;
using System.Net;
using System.Net.Http;
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
    }
}
