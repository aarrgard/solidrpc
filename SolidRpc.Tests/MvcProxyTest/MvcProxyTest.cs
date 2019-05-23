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
        public async Task TestProxyInt()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse("/swagger/v1/swagger.json");
                var content = await AssertOk(resp);
                var spec = new SwaggerParserV2().ParseSwaggerDoc(content);

                resp = await ctx.GetResponse("/MvcProxyTest/ProxyInt?i=10");
                Assert.AreEqual("10", await AssertOk(resp));

                var sp = CreateServiceProxy<IMvcProxyTest>(content);
                Assert.AreEqual(10, await sp.ProxyInt(10));
            }
        }

        private T CreateServiceProxy<T>(string swaggerConfiguration) where T : class
        {
            var sc = new ServiceCollection();
            sc.AddTransient<T,T>();
            var proxyConf = sc.GetSolidConfigurationBuilder()
                .SetGenerator<SolidProxyCastleGenerator>()
                .ConfigureInterface<T>()
                .ConfigureAdvice<ISolidRpcProxyConfig>();
            proxyConf.SwaggerConfiguration = swaggerConfiguration;
            proxyConf.Enabled = true;

            sc.GetSolidConfigurationBuilder().AddAdvice(typeof(SolidRpcProxyAdvice<,,>));

            var sp = sc.BuildServiceProvider();
            return sp.GetRequiredService<T>();
        }
    }
}