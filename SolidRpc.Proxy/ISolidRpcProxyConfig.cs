using SolidProxy.Core.Configuration;
using System;

namespace SolidRpc.Proxy
{
    /// <summary>
    /// Configures the proxy.
    /// </summary>
    public interface ISolidRpcProxyConfig : ISolidProxyInvocationAdviceConfig
    {
        /// <summary>
        /// The root address. This address overrides the host and port 
        /// in the configuration.
        /// </summary>
        Uri RootAddress { get; set; }

        /// <summary>
        /// Sets the open api configuration to use.
        /// </summary>
        string OpenApiConfiguration { get; set; }
    }
}
