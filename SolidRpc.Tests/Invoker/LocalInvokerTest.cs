using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.Core.Proxy;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.OpenApi.Binder;

namespace SolidRpc.Tests.Invoker
{
    /// <summary>
    /// Tests the invokers
    /// </summary>
    public class LocalInvokerTest : TestBase
    {
        /// <summary>
        /// 
        /// </summary>
        public interface ITestInterface
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="callerType"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task<int> DoXAsync(string callerType, CancellationToken cancellation = default(CancellationToken));
        }
        /// <summary>
        /// 
        /// </summary>
        public class TestImplementation : ITestInterface
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="callerType"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            public Task<int> DoXAsync(string callerType, CancellationToken cancellation = default(CancellationToken))
            {
                Assert.AreEqual(callerType, SolidProxyInvocationImplAdvice.CurrentInvocation?.Caller?.GetType()?.Name);
                return Task.FromResult(4711);
            }
        }

        /// <summary>
        /// Tests the local invoker
        /// </summary>
        [Test]
        public async Task TestLocalInvokerInternalService()
        {
            var sb = new ConfigurationBuilder();
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(sb.Build());
            sc.AddLogging(ConfigureLogging);
            sc.AddSolidRpcSingletonServices();
            sc.AddTransient<ITestInterface, TestImplementation>();
            var sp = sc.BuildServiceProvider();
            var invoker = sp.GetRequiredService<IInvoker<ITestInterface>>();
            var result = await invoker.InvokeAsync(o => o.DoXAsync(null, CancellationToken.None), opt => opt.SetTransport(LocalHandler.TransportType));
            Assert.AreEqual(4711, result);
        }

        /// <summary>
        /// Tests the local invoker
        /// </summary>
        [Test]
        public async Task TestLocalInvokerExternalService()
        {
            var sb = new ConfigurationBuilder();
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(sb.Build());
            sc.AddLogging(ConfigureLogging);
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcSingletonServices();

            var openApiSpec = sc.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            sc.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestImplementation), conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                return true;
            });

            var sp = sc.BuildServiceProvider();
            var invoker = sp.GetRequiredService<IInvoker<ITestInterface>>();
            var result = await invoker.InvokeAsync(o => o.DoXAsync("LocalHandler", CancellationToken.None), opt => opt.SetTransport(LocalHandler.TransportType));
            Assert.AreEqual(4711, result);
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestLocalInvokerExternalServiceWithSecurityKey()
        {
            var sb = new ConfigurationBuilder();
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(sb.Build());
            sc.AddLogging(ConfigureLogging);
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcSingletonServices();

            var openApiSpec = sc.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            sc.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestImplementation), conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SetSecurityKey(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
                return true;
            });

            var sp = sc.BuildServiceProvider();

            // we should not be able to invoke the service directly
            var proxy = sp.GetRequiredService<ITestInterface>();
            await proxy.DoXAsync("ITestInterfaceProxy");

            try
            {
                // This shold not work
                await ((ISolidProxy)proxy).InvokeAsync(this, typeof(ITestInterface).GetMethod(nameof(ITestInterface.DoXAsync)), null, null);
                Assert.Fail();
            }
            catch (UnauthorizedException)
            {
                // this is the way we want it.
            }

            var invoker = sp.GetRequiredService<IInvoker<ITestInterface>>();
            var result = await invoker.InvokeAsync(o => o.DoXAsync("LocalHandler", CancellationToken.None), opt => opt.SetTransport(LocalHandler.TransportType));
            Assert.AreEqual(4711, result);
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestInvokeLocalImplementationWithProxyAdvice()
        {
            var ms = new MemoryQueueBus();
            var sb = new ConfigurationBuilder();
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(sb.Build());
            sc.AddLogging(ConfigureLogging);
            sc.AddSingleton(ms);
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcSingletonServices();
            var openApiSpec = sc.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            sc.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestImplementation), conf =>
            {
                conf.ProxyTransportType = MemoryQueueHandler.TransportType;
                conf.OpenApiSpec = openApiSpec;
                var c = conf.SetQueueTransport<IMemoryQueueTransport>(InvocationStrategy.Invoke);
                conf.SetQueueTransportInboundHandler<IMemoryQueueTransport>("generic");
                return true;
            });

            var sp = sc.BuildServiceProvider();
            var mbs = sp.GetRequiredService<IMethodBinderStore>().MethodBinders;
            // we should not be able to invoke the service directly

            // use proxy
            var proxy = sp.GetRequiredService<ITestInterface>();
            await proxy.DoXAsync("MemoryQueueHandler");
            Assert.AreEqual(1, await ms.DispatchAllMessagesAsync());


        }
    }
}
