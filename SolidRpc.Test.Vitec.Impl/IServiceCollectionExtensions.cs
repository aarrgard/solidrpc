using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Test.Vitec.Services;

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
        public static IServiceCollection AddVitec(this IServiceCollection services)
        {
            services.AddSolidRpcBindings(typeof(IEstate).Assembly, typeof(EstateImpl).Assembly);
            return services;
        }
    }
}
