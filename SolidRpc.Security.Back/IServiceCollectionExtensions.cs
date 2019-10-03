﻿using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Security.Back.InternalServices;
using SolidRpc.Security.Back.Services;
using SolidRpc.Security.Back.Services.Facebook;
using SolidRpc.Security.Back.Services.Google;
using SolidRpc.Security.Back.Services.Microsoft;
using SolidRpc.Security.Services;
using SolidRpc.Security.Services.Facebook;
using SolidRpc.Security.Services.Google;
using SolidRpc.Security.Services.Microsoft;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

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
        public static IServiceCollection AddSolidRpcSecurityBackend(this IServiceCollection services, Action<IServiceProvider, SolidRpcSecurityOptions> configurator = null)
        {
            services.AddSingleton<IOpenIDKeyStore, OpenIDKeyStore>();
            services.AddSingleton<IAccessTokenFactory, AccessTokenFactory>();

            if (configurator != null)
            {
                services.AddSingleton(sp =>
                {
                    var opts = new SolidRpcSecurityOptions();
                    configurator(sp, opts);
                    return opts;
                });
            }

            services.AddSolidRpcBindings(typeof(ISolidRpcSecurity), typeof(SolidRpcSecurity));
            services.GetSolidRpcContentStore().AddMapping(
                "/.well-known/openid-configuration", 
                (sp) => sp.GetRequiredService<IMethodBinderStore>().GetUrlAsync<ISolidRpcSecurity>(o => o.OAuth2Discovery(CancellationToken.None)));
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
            services.AddSolidRpcSecurityBackend();

            services.AddSingleton(sp =>
            {
                var mc = new MicrosoftOptions();
                configurator(sp, mc);
                return mc;
            });

            services.AddSolidRpcBindings(typeof(IMicrosoftRemote), null, null, (sp, uri, mi) => Task.FromResult(uri));
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
            services.AddSolidRpcSecurityBackend();

            services.AddSingleton(sp =>
            {
                var fbc = new FacebookOptions();
                configurator(sp, fbc);
                return fbc;
            });

            services.AddSolidRpcBindings(typeof(IFacebookRemote), null, null, (sp, uri, mi) => Task.FromResult(uri));
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
            services.AddSolidRpcSecurityBackend();

            services.AddSingleton(sp =>
            {
                var gc = new GoogleOptions();
                configurator(sp, gc);
                return gc;
            });

            services.AddSolidRpcBindings(typeof(IGoogleLocal), typeof(GoogleLocal));
            services.AddSolidRpcBindings(typeof(IGoogleRemote), null, null, GoogleBaseApiResolver);
            services.AddTransient<ILoginProvider>(sp => sp.GetRequiredService<GoogleLocal>());
            return services;
        }

        private static async Task<Uri> GoogleBaseApiResolver(IServiceProvider serviceProvider, Uri originalUri, MethodInfo methodInfo)
        {
            if(methodInfo != null)
            {
                if (methodInfo.Name == nameof(IGoogleRemote.OpenIdConfiguration))
                {
                    return originalUri;
                }
                var bindingStore = serviceProvider.GetRequiredService<IMethodBinderStore>();
                var bindingInfo = bindingStore.GetMethodBinding(methodInfo);

                var openIdConf = await serviceProvider.GetRequiredService<IGoogleRemote>().OpenIdConfiguration();

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