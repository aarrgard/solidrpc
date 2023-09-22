using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Linq;
using System.Text;
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

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public QueueInvokerTest()
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
            public string Value { get; set; }
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
            Task<string> DoYAsync(ComplexStruct myStruct, CancellationToken cancellation = default(CancellationToken));
        }

        /// <summary>
        /// 
        /// </summary>
        public class TestImplementation : ITestInterface
        {
            /// <summary>
            /// Constructs a new instance
            /// </summary>
            /// <param name="logger"></param>
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
            public Task<string> DoYAsync(ComplexStruct myStruct, CancellationToken cancellation = default(CancellationToken))
            {
                Assert.IsTrue(SolidProxyInvocationImplAdvice.CurrentInvocation.Caller is MemoryQueueHandler, "Caller is not a memory handler.");
                //Assert.AreEqual(2, InvocationOptions.Current.Priority);
                Logger.LogTrace("DoYAsync");
                _doYInvocations++;
                return Task.FromResult(myStruct.Value);
            }
        }

        private void ConfigureQueueTransport(ISolidRpcOpenApiConfig conf, bool addInboundHandler)
        {
            conf.SetProxyTransportType<IMemoryQueueTransport>();
            conf.SetInvokerTransport<IHttpTransport, IMemoryQueueTransport>();
            var t = conf.ConfigureTransport<IMemoryQueueTransport>().SetMessagePriority(2);
            if (addInboundHandler)
            {
                t.SetInboundHandler("generic");
            }
        }

        /// <summary>
        /// Configures the server services
        /// </summary>
        /// <param name="serverServices"></param>
        public override void ConfigureServerServices(IServiceCollection serverServices)
        {
            base.ConfigureServerServices(serverServices);
            var openApiSpec = serverServices.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            serverServices.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestImplementation), conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SetSecurityKey(SecKey.ToString());
                ConfigureQueueTransport(conf, true);
                return true;
            });
            //services.AddAzTableQueue("SolidRpcAzTableConnection", "generic");
        }

        /// <summary>
        /// Configures the client services
        /// </summary>
        /// <param name="clientServices"></param>
        /// <param name="baseAddress"></param>
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
                .WriteAsJsonString();
            clientServices.AddSolidRpcBindings(typeof(ITestInterface), null, conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SetSecurityKey(SecKey.ToString());
                ConfigureQueueTransport(conf, false);
                return true;
            });
        }

        /// <summary>
        /// The security key
        /// </summary>
        public Guid SecKey { get; }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestQueueInvokerSimpleInvocation()
        {
            _doYInvocations = 0;
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ITestInterface>>();

                var sb = new StringBuilder();
                for (int i = 0; i < 1 * 1024 * 1024; i++)
                {
                    sb.Append((char)('a' + (i % 25)));
                }
                var value = sb.ToString();
                string res = null;
                int count = 2;
                for (int i = 0; i < count; i++)
                {
                    res = await invoker.InvokeAsync(o => o.DoYAsync(new ComplexStruct() { Value = value }, CancellationToken.None));
                }

                // wait for the handler queues to complete
                var numDispatched = await MemoryQueueBus.DispatchAllMessagesAsync();
                Assert.AreEqual(count, numDispatched);

                Assert.AreEqual(count, _doYInvocations);
                for (int i = 0; i < count; i++)
                {
                    res = await invoker.InvokeAsync(o => o.DoYAsync(new ComplexStruct() { Value = value }, CancellationToken.None), opt => opt.SetTransport(HttpHandler.TransportType));
                }

                // wait for the handler queues to complete
                numDispatched = await MemoryQueueBus.DispatchAllMessagesAsync();
                Assert.AreEqual(count, numDispatched);

                Assert.AreEqual(count * 2, _doYInvocations);
            }
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestQueueProxyInvocation()
        {
            _doYInvocations = 0;
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var invoker = ctx.ClientServiceProvider.GetRequiredService<ITestInterface>();

                await invoker.DoYAsync(new ComplexStruct() { Value = "test" });

                await MemoryQueueBus.DispatchAllMessagesAsync();

                Assert.AreEqual(1, _doYInvocations);


                Abstractions.OpenApi.Http.IHttpRequest invocation;
                var frontInvoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ITestInterface>>();
                await frontInvoker.InvokeAsync(o => o.DoYAsync(new ComplexStruct() { Value = "test" }, CancellationToken.None), opt => opt.SetTransport(MemoryQueueHandler.TransportType).AddPreInvokeCallback(req =>
                {
                    invocation = req;
                    return Task.CompletedTask;
                }));

            }
        }
    }
}
