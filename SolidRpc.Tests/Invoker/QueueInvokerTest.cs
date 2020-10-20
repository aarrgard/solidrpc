using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Generic;
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
                var caller = SolidProxyInvocationImplAdvice.CurrentInvocation.Caller;
                if(false == caller is QueueHandler)
                {
                    throw new Exception("Caller is not a queue handler.");
                }
                Logger.LogTrace("DoYAsync");
                _doYInvocations++;
                return Task.FromResult(myStruct.Value);
            }
        }

        private void ConfigureQueueTransport(ISolidRpcOpenApiConfig conf, bool addInboundHandler)
        {
            conf.ProxyTransportType = "MemoryQueue";
            conf.SetHttpTransport(InvocationStrategy.Forward);
            conf.SetQueueTransport<MemoryQueueHandler>();
            if (addInboundHandler)
            {
                conf.SetQueueTransportInboundHandler("generic");
            }
        }

        public override void ConfigureServerServices(IServiceCollection services)
        {
            base.ConfigureServerServices(services);
            var openApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            services.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestImplementation), conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SetSecurityKey(SecKey.ToString());
                ConfigureQueueTransport(conf, true);
                return true;
            });
            //services.AddAzTableQueue("SolidRpcAzTableConnection", "generic");
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
                .WriteAsJsonString();
            clientServices.AddSolidRpcBindings(typeof(ITestInterface), null, conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SetSecurityKey(SecKey.ToString());
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
                var tasks = ctx.ServerServiceProvider.GetRequiredService<IEnumerable<IMethodBindingHandler>>().Select(o => o.FlushQueuesAsync());
                await Task.WhenAll(tasks);

                Assert.AreEqual(count, _doYInvocations);
                for (int i = 0; i < count; i++)
                {
                    res = await invoker.InvokeAsync(o => o.DoYAsync(new ComplexStruct() { Value = value }, CancellationToken.None), InvocationOptions.Http);
                }

                // wait for the handler queues to complete
                tasks = ctx.ServerServiceProvider.GetRequiredService<IEnumerable<IMethodBindingHandler>>().Select(o => o.FlushQueuesAsync());
                await Task.WhenAll(tasks);

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

                Assert.AreEqual(1, _doYInvocations);


                Abstractions.OpenApi.Http.IHttpRequest invocation;
                var frontInvoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ITestInterface>>();
                await frontInvoker.InvokeAsync(o => o.DoYAsync(new ComplexStruct() { Value = "test" }, CancellationToken.None), InvocationOptions.MemoryQueue.AddPreInvokeCallback(req =>
                {
                    invocation = req;
                    return Task.CompletedTask;
                }));

            }
        }
    }
}
