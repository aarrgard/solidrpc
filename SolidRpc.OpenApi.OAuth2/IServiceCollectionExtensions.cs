using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.OpenApi.OAuth2.InternalServices;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the oauth2 extensions
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the oauth2 services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcOAuth2(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorityFactory, AuthorityFactoryImpl>();
            return services;
        }

    }
}
