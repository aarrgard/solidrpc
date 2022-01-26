using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.OpenApi.OAuth2.InternalServices;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the oauth2 extensions
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        private static AuthorityConfigurator AuthorityConfigurator = new AuthorityConfigurator();

        /// <summary>
        /// Adds the oauth2 services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcOAuth2(this IServiceCollection services, Action<IAuthority> conf = null)
        {
            services.AddSolidRpcSingletonServices();
            services.AddTransient(sp => sp.GetRequiredService<ISolidRpcAuthorization>().CurrentPrincipal);
            services.AddTransient<IPrincipal>(sp => sp.GetRequiredService<ClaimsPrincipal>());
            services.AddSingletonIfMissing<IAuthorityFactory>(sp => new AuthorityFactoryImpl(
                sp.GetRequiredService<IHttpClientFactory>(),
                sp.GetRequiredService<ISerializerFactory>(),
                AuthorityConfigurator));
            if(conf != null)
            {
                AuthorityConfigurator.Configure(conf);
            }
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
