using SolidRpc.OpenApi.SwaggerUI.Services;

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
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSoldRpcSwaggerUI(this IServiceCollection services)
        {
            var openApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ISwaggerUI));
            services.AddSolidRpcBindings(typeof(ISwaggerUI), typeof(SwaggerUI), openApiSpec.WriteAsJsonString());
            return services;
        }
    }
}
