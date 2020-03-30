using SolidProxy.Core.Configuration;
using System;

namespace SolidRpc.Abstractions.OpenApi.Proxy
{
    /// <summary>
    /// Interface used to configure the rate limits for an invocation
    /// </summary>
    public interface ISolidRpcRateLimitConfig : ISolidProxyInvocationAdviceConfig
    {
        /// <summary>
        /// The resource name to use for the rate limiting scheme
        /// </summary>
        string ResourceName { get; set; }

        /// <summary>
        /// The amount of time to wait for the resource to become available.
        /// </summary>
        TimeSpan Timeout { get; set; }
    }
}
