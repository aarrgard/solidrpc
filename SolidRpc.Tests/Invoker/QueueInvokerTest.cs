using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Invoker
{
    /// <summary>
    /// Tests the invokers
    /// </summary>
    public class QueueInvokerTest : WebHostTest
    {
        public QueueInvokerTest()
        {
            SecKey = Guid.NewGuid();
        }

        /// <summary>
        /// 
        /// </summary>
        public interface ITestInterface
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task<int> DoXAsync(CancellationToken cancellation = default(CancellationToken));
        }

        /// <summary>
        /// 
        /// </summary>
        public class TestImplementation : ITestInterface
        {
            public TestImplementation(ILogger<TestImplementation> logger)
            {
                Logger = logger;
            }
            private ILogger Logger { get; }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            public Task<int> DoXAsync(CancellationToken cancellation = default(CancellationToken))
            {
                Logger.LogTrace("DoXAsync");
                return Task.FromResult(4711);
            }
        }

        public override void ConfigureServerServices(IServiceCollection services)
        {
            base.ConfigureServerServices(services);
            var openApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            services.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestImplementation), conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SecurityKey = new KeyValuePair<string, string>(SecKey.ToString(), SecKey.ToString());
                conf.SetQueueTransportConnectionString(
                    services.GetSolidRpcService<IConfiguration>(),
                    "SolidRpcQueueConnection");
                return true;
            });

        }

        public override void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {
            base.ConfigureClientServices(clientServices, baseAddress);
            var openApiSpec = clientServices.GetSolidRpcOpenApiParser()
                .CreateSpecification(typeof(ITestInterface))
                .SetBaseAddress(baseAddress)
                .WriteAsJsonString();
            clientServices.AddSolidRpcBindings(typeof(ITestInterface), null, conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SecurityKey = new KeyValuePair<string, string>(SecKey.ToString(), SecKey.ToString());
                conf.SetQueueTransportInboundHandler("generic");
                conf.SetQueueTransportConnectionString(
                    clientServices.GetSolidRpcService<IConfiguration>(),
                    "SolidRpcQueueConnection");
                return true;
            });
        }

        public Guid SecKey { get; }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test, Ignore("Requires connection string")]
        public async Task TestQueueInvokerSimpleInvocation()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var invoker = ctx.ClientServiceProvider.GetRequiredService<IQueueInvoker<ITestInterface>>();

                int res = 0;
                for(int i = 0; i < 100; i++)
                {
                    res = await invoker.InvokeAsync(o => o.DoXAsync(CancellationToken.None));
                }

                //await Task.Delay(10000);
                Assert.AreEqual(4711, res);
            }
        }
    }
}
