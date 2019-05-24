using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.Proxy;
using SolidRpc.Swagger.Model.V2;
using SolidRpc.Tests.Generated.Local.Services;

namespace SolidRpc.Tests.MvcProxyTest
{
    /// <summary>
    /// Tests sending data back and forth between client and server.
    /// </summary>
    public class MvcProxyTest : WebHostMvcTest
    {
        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyBoolean()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse("/MvcProxyTest/ProxyBoolean?b=true");
                Assert.AreEqual("true", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual(true, await sp.ProxyBoolean(true));
                Assert.AreEqual(false, await sp.ProxyBoolean(false));
            }
        }

        /// <summary>
        /// Sends a short back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyShort()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse("/MvcProxyTest/ProxyShort?s=10");
                Assert.AreEqual("10", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual((short)10, await sp.ProxyShort(10));
                Assert.AreEqual((short)11, await sp.ProxyShort(11));
            }
        }

        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyInt()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse("/MvcProxyTest/ProxyInt?i=10");
                Assert.AreEqual("10", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual(10, await sp.ProxyInt(10));
                Assert.AreEqual(11, await sp.ProxyInt(11));
            }
        }

        /// <summary>
        /// Sends a long back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyLong()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse("/MvcProxyTest/ProxyLong?l=10");
                Assert.AreEqual("10", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual(10l, await sp.ProxyLong(10));
                Assert.AreEqual(11l, await sp.ProxyLong(11));
            }
        }

        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyString()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse("/MvcProxyTest/ProxyString?s=testar");
                Assert.AreEqual("\"testar\"", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual("testar", await sp.ProxyString("testar"));
                Assert.AreEqual("testar2", await sp.ProxyString("testar2"));
            }
        }

        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyGuid()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var guid = Guid.NewGuid();
                var resp = await ctx.GetResponse($"/MvcProxyTest/ProxyGuid?g={guid.ToString()}");
                Assert.AreEqual($"\"{guid.ToString()}\"", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual(guid, await sp.ProxyGuid(guid));
            }
        }


        private async Task<T> CreateServiceProxy<T>(TestHostContext ctx) where T:class
        {
            var resp = await ctx.GetResponse("/swagger/v1/swagger.json");
            var swaggerConfiguration = await AssertOk(resp);

            var sc = new ServiceCollection();
            sc.AddTransient<T,T>();
            var proxyConf = sc.GetSolidConfigurationBuilder()
                .SetGenerator<SolidProxyCastleGenerator>()
                .ConfigureInterface<T>()
                .ConfigureAdvice<ISolidRpcProxyConfig>();
            proxyConf.SwaggerConfiguration = swaggerConfiguration;

            sc.GetSolidConfigurationBuilder().AddAdvice(typeof(SolidRpcProxyAdvice<,,>));

            var sp = sc.BuildServiceProvider();
            return sp.GetRequiredService<T>();
        }
    }
}