using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.OpenApi.OAuth2.InternalServices;
using System;
using System.Reflection;

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
            services.AddSolidRpcSingletonServices();
            services.AddSingletonIfMissing<IAuthorityFactory, AuthorityFactoryImpl>();
            return services;
        }

        /// <summary>
        /// Adds the oauth2 services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="localAuthority"></param>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcOAuth2Local(this IServiceCollection services, Uri localAuthority, Action<IAuthorityLocal> conf)
        {
            services.AddSingletonIfMissing<IAuthorityFactory, AuthorityFactoryImpl>();
            services.AddSingletonIfMissing(o =>
            {
                var auth = o.GetRequiredService<IAuthorityFactory>().GetLocalAuthority(localAuthority);
                conf(auth);
                return auth;
            });
            return services;
        }

    }
}
