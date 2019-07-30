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
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcSwaggerUI(this IServiceCollection services)
        {
            var openApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ISwaggerUI));
            var strOpenApiSpec = openApiSpec.WriteAsJsonString();
            services.AddSolidRpcBindings(typeof(ISwaggerUI), typeof(SwaggerUI), strOpenApiSpec);
            services.GetSolidRpcStaticContent().AddContent(typeof(SwaggerUI).Assembly, "www", openApiSpec.BasePath);
            return services;
        }
    }
}
