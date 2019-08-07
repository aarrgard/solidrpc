using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Test.Petstore.Impl;
using SolidRpc.Test.Petstore.Services;

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
        public static IServiceCollection AddPetstore(this IServiceCollection services, BaseUriTransformer baseUriTransformer = null)
        {
            services.AddSolidRpcBindings(typeof(IPet).Assembly, typeof(PetImpl).Assembly);
            services.GetSolidRpcStaticContent().AddContent(typeof(PetImpl).Assembly, "www", "/");
            return services;
        }
    }
}
