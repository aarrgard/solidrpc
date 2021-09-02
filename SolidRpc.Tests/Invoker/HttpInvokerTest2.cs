using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Invoker;
using System;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Http;
using Microsoft.AspNetCore.Http;
using SolidRpc.Abstractions.OpenApi.Transport;

namespace SolidRpc.Tests.Invoker
{
    /// <summary>
    /// Tests the invokers
    /// </summary>
    public class HttpInvokerTest2 : WebHostTest
    {
        private bool visitedPostCallback;
        private bool visitedPreCallback;

        /// <summary>
        /// 
        /// </summary>
        public class ComplexStruct
        {
            /// <summary>
            /// 
            /// </summary>
            public int Value { get; set; }

            /// <summary>
            /// The char set
            /// </summary>
            public string ContentType { get; set; }
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
            Task<ComplexStruct> DoXAsync(ComplexStruct myStruct, CancellationToken cancellation = default(CancellationToken));
        }

        /// <summary>
        /// 
        /// </summary>
        public class TestImplementation : ITestInterface
        {
            public TestImplementation(IHttpContextAccessor httpContextAccessor)
            {
                HttpContext = httpContextAccessor.HttpContext;
            }

            public HttpContext HttpContext { get; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="myStruct"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            public Task<ComplexStruct> DoXAsync(ComplexStruct myStruct, CancellationToken cancellation = default(CancellationToken))
            {
                myStruct.ContentType = HttpContext.Request.Headers["Content-Type"];
                return Task.FromResult(myStruct);
            }
        }

        /// <summary>
        /// Configures the server services
        /// </summary>
        /// <param name="services"></param>
        public override void ConfigureServerServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
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
                conf.ConfigureTransport<IHttpTransport>().AddHttpTransportPreInvokeCallback(PreInvokeCallback);
                conf.ConfigureTransport<IHttpTransport>().AddHttpTransportPostInvokeCallback(PostInvokeCallback);
                return true;
            });

            base.ConfigureClientServices(clientServices, baseAddress);
        }

        private Task PostInvokeCallback(IHttpResponse arg)
        {
            visitedPreCallback = true;
            return Task.CompletedTask;
        }

        private Task PreInvokeCallback(IHttpRequest arg)
        {
            visitedPostCallback = true;
            return Task.CompletedTask;
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

                this.visitedPreCallback = false;
                this.visitedPostCallback = false;

                //
                // test using proxy
                //
                var ti = ctx.ClientServiceProvider.GetRequiredService<ITestInterface>();
                var res = await ti.DoXAsync(new ComplexStruct() { Value = 4711 });
                Assert.AreEqual(4711, res.Value);
                Assert.IsTrue(this.visitedPreCallback);
                Assert.IsTrue(this.visitedPostCallback);

                //
                // test using invoker
                //
                this.visitedPreCallback = false;
                this.visitedPostCallback = false;
                bool visitedPreCallback = false;
                bool visitedPostCallback = false;
                var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ITestInterface>>();
                res = await invoker.InvokeAsync(o => o.DoXAsync(new ComplexStruct() { Value = 4711 }, CancellationToken.None), opt => opt.AddPreInvokeCallback(req =>
                    {
                        visitedPreCallback = true;
                        return Task.CompletedTask;
                    }).AddPostInvokeCallback(resp =>
                    {
                        visitedPostCallback = true;
                        return Task.CompletedTask;
                    }));
                Assert.AreEqual(4711, res.Value);
                Assert.IsTrue(this.visitedPreCallback);
                Assert.IsTrue(this.visitedPostCallback);
                Assert.IsTrue(visitedPreCallback);
                Assert.IsTrue(visitedPostCallback);
            }
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestCharsetInResponse()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var invoker = ctx.ClientServiceProvider.GetRequiredService<ITestInterface>();
                var res = await invoker.DoXAsync(new ComplexStruct() { Value = 4711 }, CancellationToken.None);
                Assert.AreEqual(4711, res.Value);
                Assert.AreEqual("application/json", res.ContentType);
            }
        }
    }
}
