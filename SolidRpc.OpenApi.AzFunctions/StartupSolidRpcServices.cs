﻿using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using SolidRpc.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Proxy;
using System.Reflection;
using System;

namespace SolidRpc.OpenApi.AzFunctions
{
    /// <summary>
    /// Base class to configure the solid rpc services
    /// </summary>
    public class StartupSolidRpcServices 
    {
        private static Assembly[] s_assemblies = new Assembly[0];
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurator"></param>
        public virtual void ConfigureServices(IServiceCollection services, Func<ISolidRpcOpenApiConfig, bool> configurator = null)
        {
            if (configurator == null) configurator = ConfigureAzureFunction;

            s_assemblies = new[] {
                Assembly.Load("Microsoft.IdentityModel.Tokens"),
                Assembly.Load("System.IdentityModel.Tokens.Jwt"),
            };
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;

            //
            // configure logging
            //
            if (!services.Any(o => o.ServiceType == typeof(ILogger)))
            {
                services.AddLogging(o => {
                    o.SetMinimumLevel(LogLevel.Trace);
                    //o.AddProvider(new TraceWriterLoggerProvider());
                });
            }
            services.AddHttpClient();
            services.AddSingleton<IContentTypeProvider>(new FileExtensionContentTypeProvider());
            services.AddSolidRpcSingletonServices();
            services.AddSolidRpcServices(configurator);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        protected virtual bool ConfigureAzureFunction(ISolidRpcOpenApiConfig c)
        {
            var azConfig = c.GetAdviceConfig<ISolidAzureFunctionConfig>();
            if (c.GetAdviceConfig<ISecurityPathClaimConfig>().Enabled)
            {
                azConfig.HttpAuthLevel = "anonymous";
            }
            else
            {
                azConfig.HttpAuthLevel = "function";
            }

            var method = c.Methods.First();
            if (method.DeclaringType == typeof(ISolidRpcHost))
            {
                return method.Name == nameof(ISolidRpcHost.IsAlive);
            }
            return true;
        }

        private static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            var an = new AssemblyName(args.Name);
            return s_assemblies.FirstOrDefault(o => o.GetName().Name == an.Name);
        }
    }
}
