using SolidProxy.Core.Configuration;
using SolidRpc.Abstractions.OpenApi.Binder;
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

        /// <summary>
        /// The method to transform the Uri. This delegate is invoked to determine
        /// the base Uri for the service. Supplied uri is the one obtained from
        /// the openapi config.
        /// </summary>
        MethodAddressTransformer MethodAddressTransformer { get; set; }
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
                foreach(var m in config.Methods)
                {
                    var assembly = m.DeclaringType.Assembly;
                    var assemblyName = assembly.GetName().Name;

                    var endings = new[]
                    {
                        $"{m.DeclaringType.FullName}.json",
                        $".{assemblyName}.json"
                    };

                    foreach(var ending in endings)
                    {
                        // locate config based on method name
                        var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(o => o.EndsWith(ending, StringComparison.InvariantCultureIgnoreCase));
                        if (string.IsNullOrEmpty(resourceName))
                        {
                            continue;
                        }
                        using (var s = assembly.GetManifestResourceStream(resourceName))
                        {
                            using (var sr = new StreamReader(s))
                            {
                                strConfig = sr.ReadToEnd();
                            }
                        }
                    }
                    if (strConfig == null)
                    {
                        throw new Exception($"Failed to find configuration for method using patterns {string.Join(",", endings)}.");
                    }
                }
                config.OpenApiConfiguration = strConfig;
            }
            return strConfig;
        }
    }
}
