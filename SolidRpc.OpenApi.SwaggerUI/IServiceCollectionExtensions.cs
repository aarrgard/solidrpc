using SolidRpc.Abstractions.OpenApi.Binder;
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
        /// <param name="configurator"></param>
        /// <param name="baseUriTransformer"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcSwaggerUI(this IServiceCollection services, Action<SwaggerOptions> configurator = null, MethodAddressTransformer baseUriTransformer = null)
        {
            services.AddSingleton(sp => {
                var options = new SwaggerOptions();
                configurator?.Invoke(options);
                return options;
            });
            var openApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ISwaggerUI));
            var strOpenApiSpec = openApiSpec.WriteAsJsonString();

            services.AddSolidRpcBindings(typeof(ISwaggerUI), typeof(SwaggerUI), strOpenApiSpec, baseUriTransformer);
            services.GetSolidRpcContentStore().AddContent(typeof(SwaggerUI).Assembly, "www", typeof(ISwaggerUI).Assembly);
            return services;
        }
    }
}
