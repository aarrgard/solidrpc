using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using System.Linq;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.Abstractions.OpenApi.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace SolidRpc.Tests.Invoker
{
    /// <summary>
    /// Tests the invokers
    /// </summary>
    public class HttpInvokerTest3 : WebHostTest
    {
        /// <summary>
        /// A complex type
        /// </summary>
        public class ComplexType { }

        /// <summary>
        /// 
        /// </summary>
        public interface ITestInterfaceFront
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ct"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task DoXAsync(ComplexType ct, CancellationToken cancellation = default(CancellationToken));
        }

        /// <summary>
        /// 
        /// </summary>
        public interface ITestInterfaceBack
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ct"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task DoYAsync(ComplexType ct, CancellationToken cancellation = default(CancellationToken));
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
            /// <param name="ct"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            public Task DoXAsync(ComplexType ct, CancellationToken cancellation = default(CancellationToken))
            {
                var caller = SolidProxyInvocationImplAdvice.CurrentInvocation.Caller.GetType().Name;
                Assert.IsTrue(new[] { "ITestInterfaceFrontProxy", "LocalHandler" }.Contains(caller));
                return Back.DoYAsync(ct, cancellation);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class TestImplementationBack : ITestInterfaceBack
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ct"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            public Task DoYAsync(ComplexType ct, CancellationToken cancellation = default(CancellationToken))
            {
                var caller = SolidProxyInvocationImplAdvice.CurrentInvocation.Caller.GetType().Name;
                Assert.AreEqual("HttpHandler", caller);
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Configures the server services
        /// </summary>
        /// <param name="services"></param>
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

        protected override void ConfigureClientConfiguration(IConfigurationBuilder cb, Uri baseAddress)
        {
            base.ConfigureClientConfiguration(cb, baseAddress);
            cb.AddInMemoryCollection(new Dictionary<string, string>()
            {
                { $"{typeof(ITestInterfaceBack).FullName.Replace(".", ":")}:BaseUrl", baseAddress.ToString() }
            });
        }

        /// <summary>
        /// Configures the client services
        /// </summary>
        /// <param name="clientServices"></param>
        /// <param name="baseAddress"></param>
        public override void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {
            //
            // front api
            //
            var openApiSpec = clientServices.GetSolidRpcOpenApiParser()
                .CreateSpecification(typeof(ITestInterfaceFront))
                .WriteAsJsonString();
            clientServices.AddSolidRpcBindings(typeof(ITestInterfaceFront), typeof(TestImplementationFront), conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                return true;
            });

            //
            // backend api
            //
            openApiSpec = clientServices.GetSolidRpcOpenApiParser()
                .CreateSpecification(typeof(ITestInterfaceBack))
                .WriteAsJsonString();
            clientServices.AddSolidRpcBindings(typeof(ITestInterfaceBack), null, conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.SetProxyTransportType<IMemoryQueueTransport>();
                conf.SetInvokerTransport<IMemoryQueueTransport, IHttpTransport>();
                conf.ConfigureTransport<IMemoryQueueTransport>()
                    .SetInboundHandler("generic");
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
                await front.DoXAsync(new ComplexType());

                Assert.AreEqual(1, await MemoryQueueBus.DispatchAllMessagesAsync());

                var frontInvoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ITestInterfaceFront>>();
                await frontInvoker.InvokeAsync(o => o.DoXAsync(new ComplexType(), CancellationToken.None), opt => opt.SetTransport(LocalHandler.TransportType));

                Assert.AreEqual(1, await MemoryQueueBus.DispatchAllMessagesAsync());
            }
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestInvokeFromSerializedMessage()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var sp = ctx.ClientServiceProvider;
                var serializerFactory = sp.GetRequiredService<ISerializerFactory>();
                var methodInvoker = sp.GetRequiredService<IMethodInvoker>();
                var memoryQueueHandler = sp.GetRequiredService<MemoryQueueHandler>();
                
                var message = @"{""Method"":""POST"",""Uri"":""https://localhost/SolidRpc/Tests/Invoker/HttpInvokerTest3/ITestInterfaceBack/DoYAsync"",""Headers"":{""Content-Type"":[""application/json; charset=utf-8""]},""Body"":""e30=""}";

                HttpRequest httpRequest;
                serializerFactory.DeserializeFromString(message, out httpRequest);

                var request = new SolidHttpRequest();
                await request.CopyFromAsync(httpRequest, p => p);

                var reqMsg = new System.Net.Http.HttpRequestMessage();
                request.CopyTo(reqMsg);

                var resp = await methodInvoker.InvokeAsync(sp, memoryQueueHandler, request);

            }
        }

    }
}
