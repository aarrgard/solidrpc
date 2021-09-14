using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Services.RateLimit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.RateLimit
{
    /// <summary>
    /// Tests security functionality.
    /// </summary>
    public class RateLimitSingeltonTest : WebHostTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientServices"></param>
        /// <param name="baseAddress"></param>
        public override void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {
            base.ConfigureClientServices(clientServices, baseAddress);
            clientServices.AddSolidRpcRemoteBindings<ISolidRpcRateLimit>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public override void ConfigureServerServices(IServiceCollection services)
        {
            base.ConfigureServerServices(services);
            services.AddSolidRpcServices(o => true);
        }

        /// <summary>
        /// Tests the web host
        /// </summary>
        [Test]
        public async Task TestRateLimit()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await ctx.StartAsync();

                var rateLimitInterface = ctx.ClientServiceProvider.GetRequiredService<ISolidRpcRateLimit>();

                var key = Guid.NewGuid().ToString();

                var token1 = await rateLimitInterface.GetSingeltonTokenAsync(key, new TimeSpan(0, 0, 0));
                Assert.AreNotEqual(Guid.Empty, token1.Id);

                var token2 = await rateLimitInterface.GetSingeltonTokenAsync(key, new TimeSpan(0, 0, 0));
                Assert.AreEqual(Guid.Empty, token2.Id);

                await rateLimitInterface.ReturnRateLimitTokenAsync(token1);

                token2 = await rateLimitInterface.GetSingeltonTokenAsync(key, new TimeSpan(0, 0, 0));
                Assert.AreNotEqual(Guid.Empty, token2.Id);
            }
        }

        /// <summary>
        /// Tests the web host
        /// </summary>
        [Test]
        public void TestRateLimitConfig()
        {
            var sc = new ServiceCollection();
            var cb = sc.GetConfigurationBuilder(() => new ConfigurationBuilder(), conf => new ChainedConfigurationSource() { Configuration = conf });
            cb.AddInMemoryCollection(new Dictionary<string, string>()
            {
                { "SolidRpc:SecurityKey", "A"},
                { "SolidRpc:Abstractions:Services:RateLimit:SecurityKey", "B"},
                { "SolidRpc:Abstractions:Services:RateLimit:BaseUrl", "http://test.test/test/"}
            });

            sc.BuildConfiguration(() => new ConfigurationBuilder());

            sc.AddLogging(ConfigureLogging);
            sc.AddHttpClient();
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcServices();
            var sp = sc.BuildServiceProvider();

            var rlinvoker = sp.GetRequiredService<IInvoker<ISolidRpcRateLimit>>();
            var mb = rlinvoker.GetMethodBinding(o => o.GetRateLimitSettingsAsync(CancellationToken.None));
            var opAddr = mb.Transports.OfType<IHttpTransport>().First().OperationAddress;
            Assert.AreEqual("http://test.test/test/SolidRpc/Abstractions/Services/RateLimit/ISolidRpcRateLimit/GetRateLimitSettingsAsync", opAddr.ToString());
            Assert.AreEqual("B", mb.GetSolidProxyConfig<ISecurityKeyConfig>().SecurityKey.Value.Value);

            var rhinvoker = sp.GetRequiredService<IInvoker<ISolidRpcHost>>();
            mb = rhinvoker.GetMethodBinding(o => o.IsAlive(CancellationToken.None));
            opAddr = mb.Transports.OfType<IHttpTransport>().First().OperationAddress;
            Assert.AreEqual("https://localhost/SolidRpc/Abstractions/Services/ISolidRpcHost/IsAlive", opAddr.ToString());
            Assert.AreEqual("A", mb.GetSolidProxyConfig<ISecurityKeyConfig>().SecurityKey.Value.Value);

        }
    }
}

