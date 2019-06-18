using SolidProxy.Core.Configuration;

namespace SolidRpc.OpenApi.AspNetCore
{
    /// <summary>
    /// Configuration used to expose a service through the asp net core binder.
    /// </summary>
    public interface ISolidRpcAspNetCoreConfig : ISolidProxyInvocationAdviceConfig
    {
        /// <summary>
        /// Sets the open api configuration to use.
        /// </summary>
        string OpenApiConfiguration { get; set; }
    }
}
