using SolidProxy.Core.Configuration;

namespace SolidRpc.OpenApi.Binder.Proxy
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
}
