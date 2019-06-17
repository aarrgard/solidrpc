using Microsoft.Extensions.DependencyInjection;
using SolidProxy.Core.Configuration.Builder;
using SolidRpc.Proxy;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Binds all the solid rpc proxies that has an implementation on this server.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSolidRpcProxies(this IApplicationBuilder applicationBuilder)
        {
            var builder = applicationBuilder.ApplicationServices.GetService<ISolidConfigurationBuilder>();
            if(builder == null)
            {
                throw new Exception("No solid proxy configuration registered - please configure during startup.");
            }
            foreach(var ab in builder.AssemblyBuilders)
            {
                foreach(var i in ab.Interfaces)
                {
                    foreach(var m in i.Methods)
                    {
                        if(!m.IsAdviceConfigured<ISolidRpcProxyConfig>())
                        {
                            continue;
                        }
                    }
                }
            }
            return applicationBuilder;
        }
    }
}
