using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Services.RateLimit;
using SolidRpc.OpenApi.AspNetCore.Services;
using System;

using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods fro the http request
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Returns the configuration builder
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcRateLimitMemory(
            this IServiceCollection services,
            Func<ISolidRpcOpenApiConfig, bool> configurator = null)
        {
            services.AddSingleton<ISolidRpcRateLimit, SolidRpcRateLimitMemory>();
            var methods = typeof(ISolidRpcRateLimit).GetMethods();
            var openApiParser = services.GetSolidRpcOpenApiParser();
            var openApiSpec = openApiParser.CreateSpecification(methods.ToArray())
                .WriteAsJsonString();

            services.AddSolidRpcBindings(typeof(ISolidRpcRateLimit), null, conf =>
            {
                conf.OpenApiSpec = openApiSpec;
                conf.GetAdviceConfig<ISolidProxyInvocationImplAdviceConfig>().Enabled = true;
                return configurator?.Invoke(conf) ?? true;
            });
            return services;
        }
    }
}
