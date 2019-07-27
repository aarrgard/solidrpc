using SolidRpc.Abstractions.OpenApi.Proxy;
using System;

namespace SolidRpc.OpenApi.Proxy
{
    /// <summary>
    /// Configures the proxy.
    /// </summary>
    public interface ISolidRpcProxyConfig : ISolidRpcOpenApiConfig
    {
        /// <summary>
        /// The root address. This address overrides the host and port 
        /// in the configuration.
        /// </summary>
        Uri RootAddress { get; set; }
    }
}
