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
        public static IServiceCollection AddPetstore(this IServiceCollection services)
        {
            services.AddSolidRpcBindings(typeof(IPet).Assembly, typeof(PetImpl).Assembly);
            services.GetSolidRpcContentStore().AddContent(typeof(PetImpl).Assembly, $"www.images.test-override", "/images/");
            services.GetSolidRpcContentStore().AddContent(typeof(PetImpl).Assembly, "www", "/");
            return services;
        }
    }
}
