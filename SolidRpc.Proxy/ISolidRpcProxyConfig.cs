using SolidProxy.Core.Configuration;

namespace SolidRpc.Proxy
{
    /// <summary>
    /// Configures the proxy.
    /// </summary>
    public interface ISolidRpcProxyConfig : ISolidProxyInvocationAdviceConfig
    {
        /// <summary>
        /// Sets the swagger configuration to use.
        /// </summary>
        string SwaggerConfiguration { get; set; }
    }
}
