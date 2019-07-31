using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Proxy;
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
        /// <param name="baseUriTransformer"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcSwaggerUI(this IServiceCollection services, BaseUriTransformer baseUriTransformer = null)
        {
            var openApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ISwaggerUI));
            var strOpenApiSpec = openApiSpec.WriteAsJsonString();

            services.AddSolidRpcBindings(typeof(ISwaggerUI), typeof(SwaggerUI), strOpenApiSpec, baseUriTransformer);
            services.GetSolidRpcStaticContent().AddContent(typeof(SwaggerUI).Assembly, "www", openApiSpec.BaseAddress.AbsolutePath);
            return services;
        }
    }
}
