using SolidProxy.Core.Configuration;

namespace SolidRpc.Abstractions.OpenApi.Proxy
{
    /// <summary>
    /// Configures the required security claim neede to
    /// invoke the operation.
    /// </summary>
    public interface ISecurityPathClaimConfig : ISolidProxyInvocationAdviceConfig
    {
    }
}
