using SolidProxy.Core.Configuration;
using System.Collections.Generic;

namespace SolidRpc.Abstractions.OpenApi.Proxy
{
    /// <summary>
    /// Configures a security key.
    /// </summary>
    public interface ISecurityKeyConfig : ISolidProxyInvocationAdviceConfig
    {
        /// <summary>
        /// If this key is set it needs to be specified in the invocation
        /// properties in order for the invocation to be authorized on the server side.
        /// If the key is present on the client side it is added to call so that
        /// the invocation is authorized.
        /// </summary>
        KeyValuePair<string, string>? SecurityKey { get; set; }
    }
}
