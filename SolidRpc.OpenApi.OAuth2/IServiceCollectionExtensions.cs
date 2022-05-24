﻿using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.OAuth2.InternalServices;
using SolidRpc.OpenApi.OAuth2.Services;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

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
                sp.GetRequiredService<ILogger<AuthorityImpl>>(),
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
        /// <param name="conf"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcOAuth2Local(this IServiceCollection services, Action<IAuthorityLocal> conf = null)
        {
            return AddSolidRpcOAuth2Local(services, null, conf);
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
            services.AddSingletonIfMissing(sp =>
            {
                if(string.IsNullOrEmpty(localAuthority))
                {
                    localAuthority = services.GetSolidRpcOAuth2LocalIssuer();
                }
                var auth = sp.GetRequiredService<IAuthorityFactory>().GetLocalAuthority(localAuthority);
                conf?.Invoke(auth);
                return auth;
            });
            return services;
        }

        /// <summary>
        /// Returns the address to the local oauth2 issuer
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static string GetSolidRpcOAuth2LocalIssuer(this IServiceCollection services)
        {
            var baseAddress = services.GetSolidRpcService<IMethodAddressTransformer>().BaseAddress.ToString();
            if (baseAddress.EndsWith("/")) baseAddress = baseAddress.Substring(0, baseAddress.Length - 1);
            var oauth2Issuer = $"{baseAddress}/SolidRpc/Abstractions";
            return oauth2Issuer;
        }

        /// <summary>
        /// Adds the test oidc service
        /// </summary>
        /// <param name="services"></param>
        /// <param name="clientAllowedPaths"></param>
        /// <param name="userAllowedPaths"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcOidcTestImpl(this IServiceCollection services, string[] clientAllowedPaths = null, string[] userAllowedPaths = null)
        {
            services.AddScoped<ISolidRpcOidc, SolidRpcOidcTestImpl>();
            SolidRpcOidcTestImpl.ClientAllowedPaths = clientAllowedPaths ?? new[] { "/*" };
            SolidRpcOidcTestImpl.UserAllowedPaths = userAllowedPaths ?? new[] { "/*" };
            return services;
        }

    }
}
