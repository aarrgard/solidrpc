using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.Services.RateLimit;
using SolidRpc.Abstractions.Types;
using System;
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
             clientServices.AddSolidRpcRateLimit(baseAddress);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public override void ConfigureServerServices(IServiceCollection services)
        {
            base.ConfigureServerServices(services);
            services.AddSolidRpcRateLimitMemory();
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
    }
}

