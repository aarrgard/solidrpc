﻿using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Security.Back.Services;
using SolidRpc.Security.Back.Services.Facebook;
using SolidRpc.Security.Back.Services.Google;
using SolidRpc.Security.Back.Services.Microsoft;
using SolidRpc.Security.Services;
using SolidRpc.Security.Services.Facebook;
using SolidRpc.Security.Services.Google;
using SolidRpc.Security.Services.Microsoft;
using SolidRpc.Security.Services.Oidc;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the service collections
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the rpc services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurator"></param>
        /// <param name="addStaticContent"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcSecurityBackend(
            this IServiceCollection services, 
            Action<IServiceProvider, SolidRpcSecurityOptions> configurator = null,
            Func<ISolidRpcOpenApiConfig, bool> mbConfigurator = null)
        {
            if(!services.Any(o => o.ServiceType == typeof(IAuthorityLocal)))
            {
                throw new Exception("You need to add the local authority before adding the backend");
            }

            if (configurator != null)
            {
                services.AddSingleton(sp =>
                {
                    var opts = new SolidRpcSecurityOptions();
                    configurator(sp, opts);
                    return opts;
                });
            }

            services.AddSolidRpcBindings(typeof(IOidcServer), typeof(OidcServer), o => {
                var res = mbConfigurator?.Invoke(o) ?? true;
                o.DisableSecurity();
                return res;
            });
            services.AddSolidRpcBindings(typeof(ISolidRpcSecurity), typeof(SolidRpcSecurity), mbConfigurator);
            var baseAddess = services.GetSolidRpcService<IMethodAddressTransformer>().BaseAddress;
            services.GetSolidRpcContentStore().AddMapping(
                $"{baseAddess.AbsolutePath}.well-known/openid-configuration", 
                (sp) => sp.GetRequiredService<IInvoker<IOidcServer>>().GetUriAsync(o => o.OAuth2Discovery(CancellationToken.None)));
            var sc = new SolidRpcSecurityOptions();
            configurator?.Invoke(null, sc);
            if (sc.AddStaticContent)
            {
                services.GetSolidRpcContentStore().AddContent(typeof(ILoginProvider).Assembly, "www", typeof(ISolidRpcSecurity).Assembly);
            }
            return services;
        }

        /// <summary>
        /// Adds the rpc services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcSecurityBackendMicrosoft(this IServiceCollection services, Action<IServiceProvider, MicrosoftOptions> configurator)
        {
            services.AddSingleton(sp =>
            {
                var mc = new MicrosoftOptions();
                configurator(sp, mc);
                if (string.IsNullOrEmpty(mc.Tenant))
                {
                    throw new Exception("No tenant set for Microsoft IdP");
                }
                if (mc.ClientID == Guid.Empty)
                {
                    throw new Exception("No ClientId(ApplicationId) set for Microsoft IdP");
                }
                if (string.IsNullOrEmpty(mc.ClientSecret))
                {
                    throw new Exception("No client password set for Microsoft IdP");
                }
                return mc;
            });

            services.AddSolidRpcBindings(typeof(IMicrosoftRemote), (Type)null);
            services.AddSolidRpcBindings(typeof(IMicrosoftLocal), typeof(MicrosoftLocal));
            services.AddTransient<ILoginProvider>(sp => sp.GetRequiredService<MicrosoftLocal>());

            return services;
        }

        /// <summary>
        /// Adds the rpc services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcSecurityBackendFacebook(this IServiceCollection services, Action<IServiceProvider, FacebookOptions> configurator)
        {
            if (!services.Any(o => o.ServiceType == typeof(IOidcServer)))
            {
                throw new Exception("You need to add the security backend before adding the backend provider.");
            }

            services.AddSingleton(sp =>
            {
                var fbc = new FacebookOptions();
                configurator(sp, fbc);
                return fbc;
            });

            services.AddSolidRpcBindings(typeof(IFacebookRemote), (Type)null);

            services.AddSolidRpcBindings(typeof(IFacebookLocal), typeof(FacebookLocal));
            services.AddTransient<ILoginProvider>(sp => sp.GetRequiredService<FacebookLocal>());

            return services;
        }

        /// <summary>
        /// Adds the rpc services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcSecurityBackendGoogle(this IServiceCollection services, Action<IServiceProvider, GoogleOptions> configurator)
        {
            services.AddSingleton(sp =>
            {
                var gc = new GoogleOptions();
                configurator(sp, gc);
                return gc;
            });

            services.AddSolidRpcBindings(typeof(IGoogleLocal), typeof(GoogleLocal));
            services.AddSolidRpcBindings(typeof(IGoogleRemote), null, (c) =>
            {
                c.ConfigureTransport<IHttpTransport>().SetMethodAddressTransformer(GoogleBaseApiResolver);
                return true;
            });
            services.AddTransient<ILoginProvider>(sp => sp.GetRequiredService<GoogleLocal>());
            return services;
        }

        private static Uri GoogleBaseApiResolver(IServiceProvider serviceProvider, Uri originalUri, MethodInfo methodInfo)
        {
            if(methodInfo != null)
            {
                if (methodInfo.Name == nameof(IGoogleRemote.OpenIdConfiguration))
                {
                    return originalUri;
                }

                var openIdConf = serviceProvider.GetRequiredService<IGoogleRemote>().OpenIdConfiguration().Result;

                switch(methodInfo.Name)
                {
                    case nameof(IGoogleRemote.OpenIdKeys):
                        return openIdConf.JwksUri;
                    case nameof(IGoogleRemote.Authorize):
                        return openIdConf.AuthorizationEndpoint;
                    default:
                        throw new Exception($"Cannot handle uri for method {methodInfo.Name}");
                }
            }
            return originalUri;
        }
    }
}
