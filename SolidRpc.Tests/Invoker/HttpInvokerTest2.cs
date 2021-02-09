using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Invoker;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Invoker
{
    /// <summary>
    /// Tests the invokers
    /// </summary>
    public class HttpInvokerTest2 : WebHostTest
    {
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
                return true;
            });

            base.ConfigureClientServices(clientServices, baseAddress);
        }


        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestHttpInvokerPrePostCallbacks()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                bool visitedPreCallback = false;
                bool visitedPostCallback = false;
                var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ITestInterface>>();
                var httpInvocationOptions = InvocationOptions.Http.AddPreInvokeCallback(req =>
                {
                    visitedPreCallback = true;
                    return Task.CompletedTask;
                }).AddPostInvokeCallback(resp =>
                {
                    visitedPostCallback = true;
                    return Task.CompletedTask;
                }); 
                var res = await invoker.InvokeAsync(o => o.DoXAsync(new ComplexStruct() { Value = 4711 }, CancellationToken.None), httpInvocationOptions);
                Assert.AreEqual(4711, res);
                Assert.IsTrue(visitedPreCallback);
                Assert.IsTrue(visitedPostCallback);
            }
        }

    }
}
