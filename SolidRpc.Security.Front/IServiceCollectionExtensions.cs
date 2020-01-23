using SolidProxy.Core.Configuration.Builder;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.Services;
using SolidRpc.Security.Front;
using SolidRpc.Security.Front.InternalServices;
using SolidRpc.Security.Front.Services.oidc;
using SolidRpc.Security.Services.Oidc;
using System;
using System.Linq;

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
        public static IServiceCollection AddSolidRpcSecurityFrontend(
            this IServiceCollection services, 
            Action<IServiceProvider, SolidRpcSecurityOptions> configurator = null,
            Action<ISolidMethodConfigurationBuilder> mbConfigurator = null)
        {
            services.AddSingletonIfMissing<IOpenIDKeyStore, OpenIDKeyStore>();
            services.AddSingletonIfMissing<IAccessTokenFactory, AccessTokenFactory>();

            services.AddSingletonIfMissing<ISolidRpcSecurityOptions>(sp =>
            {
                var opts = new SolidRpcSecurityOptions();
                configurator?.Invoke(sp, opts);
                //
                // if the authority is not set - determine it by getting it from the backend
                //
                if (string.IsNullOrEmpty(opts.Authority))
                {
                    var contentHandler = sp.GetRequiredService<ISolidRpcContentHandler>();
                    var pathMappings = contentHandler.GetPathMappingsAsync().Result;
                    var wellKnown = pathMappings.Where(o => o.Name == "/.well-known/openid-configuration").Select(o => o.Value).FirstOrDefault();
                    if (wellKnown == null)
                    {
                        throw new Exception("Cannot determine authority");
                    }
                    var idx = wellKnown.IndexOf("/SolidRpc/Security/Services/Oidc/discovery");
                    if (idx == -1)
                    {
                        throw new Exception("Cannot determine authority");
                    }
                    opts.Authority = wellKnown.Substring(0, idx);
                }
                if (string.IsNullOrEmpty(opts.ClientId))
                {
                    opts.ClientId = Guid.NewGuid().ToString();
                    opts.ClientSecret = Guid.NewGuid().ToString();
                }
                return opts;
            });
 
            services.AddSolidRpcBindings(typeof(IOidcClient), typeof(OidcClient), mbConfigurator);

            return services;
        }
    }
}
