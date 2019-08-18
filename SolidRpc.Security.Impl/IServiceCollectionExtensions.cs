using SolidRpc.Security.Services;
using SolidRpc.Security.Services.OAuth2.Google;
using SolidRpc.Security.Services.OAuth2.Microsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        /// <returns></returns>
        public static IServiceCollection AddSolidRpcSecurity(this IServiceCollection services)
        {
            services.AddSolidRpcSingletonServices();
            services.AddSolidRpcBindings(typeof(IOAuth2Microsoft));
            services.AddSolidRpcBindings(typeof(IOAuth2MicrosoftCallback));

            services.AddSolidRpcBindings(typeof(IOAuth2Google));

            return services;
        }
    }
}
