﻿using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using SolidRpc.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Proxy;

namespace SolidRpc.OpenApi.AzFunctions
{
    /// <summary>
    /// Base class to configure the solid rpc services
    /// </summary>
    public class StartupSolidRpcServices 
    {

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services"></param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            var azFuncHandler = services.GetAzFunctionHandler();

            //
            // configure IConfiguration
            //
            var config = (IConfiguration)services.FirstOrDefault(o => o.ServiceType == typeof(IConfiguration))?.ImplementationInstance;
            var cb = new ConfigurationBuilder();
            if(config == null)
            {
                cb.AddEnvironmentVariables();
            }
            else
            {
                cb.AddConfiguration(config);
            }
            cb.AddInMemoryCollection(new Dictionary<string, string>() {
                { ConfigurationMethodAddressTransformer.ConfigPathPrefix, azFuncHandler.HttpRouteFrontendPrefix}
            });
            services.AddSingleton<IConfiguration>(cb.Build());

            //
            // configure logging
            //
            if(!services.Any(o => o.ServiceType == typeof(ILogger)))
            {
                services.AddLogging(o => {
                    o.SetMinimumLevel(LogLevel.Trace);
                    //o.AddProvider(new TraceWriterLoggerProvider());
                });
            }
            services.AddHttpClient();
            services.AddSingleton<IContentTypeProvider>(new FileExtensionContentTypeProvider());
            services.AddSolidRpcSingletonServices();
            services.AddSolidRpcServices(ConfigureAzureFunction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        protected virtual bool ConfigureAzureFunction(ISolidRpcOpenApiConfig c)
        {
            var azConfig = c.GetAdviceConfig<ISolidAzureFunctionConfig>();
            if (c.SecurityKey == null)
            {
                azConfig.HttpAuthLevel = "function";
            }
            else
            {
                azConfig.HttpAuthLevel = "anonymous";
            }

            var method = c.Methods.First();
            if (method.DeclaringType == typeof(ISolidRpcHost))
            {
                return method.Name == nameof(ISolidRpcHost.IsAlive);
            }
            return true;
        }
    }
}
