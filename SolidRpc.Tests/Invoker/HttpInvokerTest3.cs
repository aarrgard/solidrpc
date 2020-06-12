using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;

namespace SolidRpc.Tests.Invoker
{
    /// <summary>
    /// Tests the invokers
    /// </summary>
    public class HttpInvokerTest3 : WebHostTest
    {

        /// <summary>
        /// 
        /// </summary>
        public interface ITestInterfaceFront
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task DoXAsync(CancellationToken cancellation = default(CancellationToken));
        }

        /// <summary>
        /// 
        /// </summary>
        public interface ITestInterfaceBack
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task DoYAsync(CancellationToken cancellation = default(CancellationToken));
        }

        /// <summary>
        /// 
        /// </summary>
        public class TestImplementationFront : ITestInterfaceFront
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="back"></param>
            public TestImplementationFront(ITestInterfaceBack back)
            {
                Back = back;
            }
            /// <summary>
            /// 
            /// </summary>
            private ITestInterfaceBack Back { get; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            public Task DoXAsync(CancellationToken cancellation = default(CancellationToken))
            {
                return Back.DoYAsync(cancellation);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class TestImplementationBack : ITestInterfaceBack
        {
            public Task DoYAsync(CancellationToken cancellation = default(CancellationToken))
            {
                var caller = SolidProxyInvocationImplAdvice.CurrentInvocation.Caller.GetType().Name;
                Assert.AreEqual("HttpHandler", caller);
                return Task.CompletedTask;
            }
        }

        public override void ConfigureServerServices(IServiceCollection services)
        {
            base.ConfigureServerServices(services);

            var openApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterfaceBack)).WriteAsJsonString();
            services.AddSolidRpcBindings(typeof(ITestInterfaceBack), typeof(TestImplementationBack), conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                return true;
            });

        }

        public override void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {

            var openApiSpec = clientServices.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterfaceFront)).WriteAsJsonString();
            clientServices.AddSolidRpcBindings(typeof(ITestInterfaceFront), typeof(TestImplementationFront), conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                return true;
            });


            openApiSpec = clientServices.GetSolidRpcOpenApiParser()
                .CreateSpecification(typeof(ITestInterfaceBack))
                .SetBaseAddress(baseAddress)
                .WriteAsJsonString();
            clientServices.AddSolidRpcBindings(typeof(ITestInterfaceBack), null, conf =>
            {
                conf.ProxyTransportType = MemoryQueueHandler.TransportType;
                conf.OpenApiSpec = openApiSpec;
                conf.SetHttpTransport(InvocationStrategy.Invoke);
                conf.SetQueueTransport<MemoryQueueHandler>(InvocationStrategy.Invoke);
                conf.SetQueueTransportInboundHandler("generic");
                return true;
            });

            base.ConfigureClientServices(clientServices, baseAddress);
        }


        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestProxyTransportTypes()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();
                var binders = ctx.ClientServiceProvider.GetRequiredService<IMethodBinderStore>().MethodBinders;
                var front = ctx.ClientServiceProvider.GetRequiredService<ITestInterfaceFront>();
                await front.DoXAsync();
            }
        }

    }
}
