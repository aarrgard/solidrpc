using SolidProxy.Core.Configuration;

namespace SolidRpc.Proxy
{
    /// <summary>
    /// Configures the proxy.
    /// </summary>
    public interface ISolidRpcProxyConfig : ISolidProxyInvocationAdviceConfig
    {
        /// <summary>
        /// Sets the open api configuration to use.
        /// </summary>
        string OpenApiConfiguration { get; set; }
    }
}
