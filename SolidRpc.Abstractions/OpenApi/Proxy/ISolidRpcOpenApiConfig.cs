﻿using SolidProxy.Core.Configuration;
using System;
using System.IO;
using System.Linq;

namespace SolidRpc.Abstractions.OpenApi.Proxy

{
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public interface ISolidRpcOpenApiConfig : ISolidProxyInvocationAdviceConfig
    {
        /// <summary>
        /// Sets the open api configuration to use. If not set the configuration matching
        /// the assembly name where the method is defined will be used.
        /// </summary>
        string OpenApiConfiguration { get; set; }
    }

    /// <summary>
    /// Extension methods for the interface.
    /// </summary>
    public static class ISolidRpcOpenApiConfigExtensions
    {
        /// <summary>
        /// Returns the open api configuration configured for the rpc scope.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static string GetOpenApiConfiguration(this ISolidRpcOpenApiConfig config)
        {
            var strConfig = config.OpenApiConfiguration;
            if (strConfig == null)
            {
                var assemblies = config.Methods.Select(o => o.DeclaringType.Assembly).Distinct().ToList();
                if(assemblies.Count != 1)
                {
                    throw new Exception("Configuration belongs to more than one assembly.");
                }
                // locate config base on assembly name
                var assembly = assemblies.First();
                var assemblyName = assembly.GetName().Name;
                var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(o => o.EndsWith($".{assemblyName}.json"));
                if (resourceName == null)
                {
                    throw new Exception($"The assembly({assembly.GetName()}) does not contain a swagger spec.");
                }
                using (var s = assembly.GetManifestResourceStream(resourceName))
                {
                    using (var sr = new StreamReader(s))
                    {
                        config.OpenApiConfiguration = strConfig = sr.ReadToEnd();
                    }
                }
            }
            return strConfig;
        }
    }
}