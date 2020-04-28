using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.OpenApi.Binder.Invoker;
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
        private static int _doYInvocations = 0;

        public QueueInvokerTest()
        {
            SecKey = Guid.NewGuid();
            //SecKey = Guid.Parse("53ca29a5-1b3c-40eb-ba85-23734fbaefd0");
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
                _doYInvocations++;
                return Task.FromResult(myStruct.Value);
            }
        }

        private void ConfigureQueueTransport(ISolidRpcOpenApiConfig conf, bool addInboundHandler)
        {
            conf.SetHttpTransport(InvocationStrategy.Forward);
            conf.SetQueueTransport<MemoryQueueHandler>();
            conf.SetQueueTransportInboundHandler("generic");
            conf.InvokerTransport = conf.QueueTransport.TransportType;
        }

        public override void ConfigureServerServices(IServiceCollection services)
        {
            base.ConfigureServerServices(services);
            var openApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            services.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestImplementation), conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SetSecurityKey(SecKey);
                ConfigureQueueTransport(conf, true);
                return true;
            });

        }

        public override void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {
            base.ConfigureClientServices(clientServices, baseAddress);
            clientServices.AddSolidRpcServices(conf =>
            {
                //conf.SetSecurityKey(SecKey);
                ConfigureQueueTransport(conf, false);
                return true;
            });
            var openApiSpec = clientServices.GetSolidRpcOpenApiParser()
                .CreateSpecification(typeof(ITestInterface))
                .SetBaseAddress(baseAddress)
                .WriteAsJsonString();
            clientServices.AddSolidRpcBindings(typeof(ITestInterface), null, conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SetSecurityKey(SecKey);
                ConfigureQueueTransport(conf, false);
                return true;
            });
        }

        public Guid SecKey { get; }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestQueueInvokerSimpleInvocation()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ITestInterface>>();

                int res = 0;
                int count = 3;
                for (int i = 0; i < count; i++)
                {
                    res = await invoker.InvokeAsync(o => o.DoYAsync(new ComplexStruct() { Value = 4711 }, CancellationToken.None), InvocationOptions.MemoryQueue);
                }
                Assert.AreEqual(count, _doYInvocations);
                for (int i = 0; i < count; i++)
                {
                    res = await invoker.InvokeAsync(o => o.DoYAsync(new ComplexStruct() { Value = 4711 }, CancellationToken.None), InvocationOptions.Http);
                }
                Assert.AreEqual(count*2, _doYInvocations);
                //await Task.Delay(10000);
                //Assert.AreEqual(4711, res);
            }
        }
    }
}
