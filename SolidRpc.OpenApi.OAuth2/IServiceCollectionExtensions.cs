using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.OAuth2.InternalServices;
using System;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;

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
            services.AddTransient(sp => sp.GetRequiredService<ISolidRpcAuthorization>().CurrentPrincipal);
            services.AddTransient<IPrincipal>(sp => sp.GetRequiredService<ClaimsPrincipal>());
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
        public static IServiceCollection AddSolidRpcOAuth2Local(this IServiceCollection services, string localAuthority, Action<IAuthorityLocal> conf = null)
        {
            services.AddSolidRpcOAuth2();
            services.AddSingletonIfMissing(o =>
            {
                var auth = o.GetRequiredService<IAuthorityFactory>().GetLocalAuthority(localAuthority);
                conf?.Invoke(auth);
                return auth;
            });
            return services;
        }

    }
}
