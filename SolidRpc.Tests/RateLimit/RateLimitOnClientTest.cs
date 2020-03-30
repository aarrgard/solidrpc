using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SolidRpc.OpenApi.Binder.Proxy;

namespace SolidRpc.Tests.RateLimit
{
    /// <summary>
    /// Tests security functionality.
    /// </summary>
    public class RateLimitOnClientTest : WebHostTest
    {
        /// <summary>
        /// 
        /// </summary>
        public interface ITestInterface
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            Task RateLimitedMethod();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            Task NotRateLimitedMethod();
        }
        /// <summary>
        /// 
        /// </summary>
        public class TestImplementation : ITestInterface
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public Task NotRateLimitedMethod()
            {
                return Task.CompletedTask;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public Task RateLimitedMethod()
            {
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientServices"></param>
        /// <param name="baseAddress"></param>
        public override void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {
            clientServices.GetSolidConfigurationBuilder()
                .AddAdviceDependency(typeof(SolidRpcRateLimitAdvice<,,>), typeof(SolidRpcOpenApiAdvice<,,>));

            clientServices.AddSolidRpcRateLimit(baseAddress);

            var apiSpec = clientServices.GetSolidRpcOpenApiParser()
                .CreateSpecification(typeof(ITestInterface))
                .SetBaseAddress(baseAddress)
                .WriteAsJsonString();

            clientServices.AddSolidRpcSingletonBindings<ITestInterface>(null, conf =>
            {
                conf.OpenApiSpec = apiSpec;

                var mi = conf.Methods.First();
                if (mi.Name == nameof(ITestInterface.RateLimitedMethod))
                {
                    var rateLimitConfig = conf.GetAdviceConfig<ISolidRpcRateLimitConfig>();
                    rateLimitConfig.ResourceName = $"{mi.DeclaringType.FullName}.{mi.Name}";
                    rateLimitConfig.Timeout = new TimeSpan(0, 0, 1);
                }

                return true;
            });
            base.ConfigureClientServices(clientServices, baseAddress);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public override void ConfigureServerServices(IServiceCollection services)
        {
            services.GetConfigurationBuilder(() => new ConfigurationBuilder(), _ => new ChainedConfigurationSource { Configuration = _ })
               .AddInMemoryCollection(new Dictionary<string, string>()
               {
                    { $"SolidRpcRateLimit:{typeof(ITestInterface).FullName}.{nameof(ITestInterface.RateLimitedMethod)}:MaxConcurrentCalls", "0"}
               });

            base.ConfigureServerServices(services);

            services.AddSolidRpcRateLimitMemory();

            var apiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            services.AddSolidRpcSingletonBindings<ITestInterface>(new TestImplementation(), conf =>
            {
                conf.OpenApiSpec = apiSpec;
                return true;
            });
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

                var testInterface = ctx.ClientServiceProvider.GetRequiredService<ITestInterface>();
                try
                {
                    await testInterface.RateLimitedMethod();
                    Assert.Fail();
                }
                catch (RateLimitExceededException)
                {
                }
                await testInterface.NotRateLimitedMethod();
            }
        }
    }
}

