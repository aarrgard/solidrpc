using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzQueue.Invoker;
using System;
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
            //SecKey = Guid.NewGuid();
            SecKey = Guid.Parse("53ca29a5-1b3c-40eb-ba85-23734fbaefd0");
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
            Task<int> DoYAsync(ComplexStruct myStruct, CancellationToken cancellation = default(CancellationToken));
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
            /// <param name="myStruct"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            public Task<int> DoYAsync(ComplexStruct myStruct, CancellationToken cancellation = default(CancellationToken))
            {
                Logger.LogTrace("DoYAsync");
                return Task.FromResult(myStruct.Value);
            }
        }

        public override void ConfigureServerServices(IServiceCollection services)
        {
            base.ConfigureServerServices(services);
            var openApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            services.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestImplementation), conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SetSecurityKey(SecKey);
                conf.SetQueueTransport<QueueInvocationHandler>();
                conf.SetQueueTransportInboundHandler("generic");
                return true;
            });

        }

        public override void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {
            base.ConfigureClientServices(clientServices, baseAddress);
            clientServices.AddSolidRpcServices(conf =>
            {
                //conf.SetSecurityKey(SecKey);
                conf.SetQueueTransport<QueueInvocationHandler>();
                return true;
            });
            var openApiSpec = clientServices.GetSolidRpcOpenApiParser()
                .CreateSpecification(typeof(ITestInterface))
                .SetBaseAddress(baseAddress)
                .WriteAsJsonString();
            clientServices.AddSolidRpcBindings(typeof(ITestInterface), null, conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                //conf.SetSecurityKey(SecKey);
                conf.SetQueueTransport<QueueInvocationHandler>();
                return true;
            });
        }

        public Guid SecKey { get; }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test, Ignore("Requires password")]
        public async Task TestQueueInvokerSimpleInvocation()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var invoker = ctx.ClientServiceProvider.GetRequiredService<IQueueInvoker<ITestInterface>>();
                //var invoker = ctx.ClientServiceProvider.GetRequiredService<IQueueInvoker<ISolidRpcHost>>();

                int res = 0;
                for(int i = 0; i < 1; i++)
                {
                    res = await invoker.InvokeAsync(o => o.DoYAsync(new ComplexStruct() { Value = 4711 }, CancellationToken.None)); ;
                    //await invoker.InvokeAsync(o => o.IsAlive(CancellationToken.None));
                }

                await Task.Delay(10000);
                Assert.AreEqual(4711, res);
            }
        }
    }
}
