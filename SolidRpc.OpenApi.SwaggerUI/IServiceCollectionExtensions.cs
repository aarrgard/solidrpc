using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.OpenApi.SwaggerUI;
using SolidRpc.OpenApi.SwaggerUI.Services;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods or the service collections
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the swagger UI to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsConfigurator"></param>
        /// <param name="apiConfigurator"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcSwaggerUI(
            this IServiceCollection services, 
            Action<SwaggerOptions> optionsConfigurator = null, 
            Func<ISolidRpcOpenApiConfig, bool> apiConfigurator = null)
        {
            services.AddSingleton(sp => {
                var options = new SwaggerOptions();
                optionsConfigurator?.Invoke(options);
                return options;
            });

            services.AddSolidRpcBindings(typeof(ISwaggerUI), typeof(SwaggerUI), c => { return apiConfigurator?.Invoke(c) ?? true; }); 
            services.GetSolidRpcContentStore().AddContent(typeof(SwaggerUI).Assembly, "www", typeof(ISwaggerUI).Assembly);
            return services;
        }
    }
}
