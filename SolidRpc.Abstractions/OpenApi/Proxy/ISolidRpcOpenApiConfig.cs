using SolidProxy.Core.Configuration;
using SolidRpc.Abstractions.OpenApi.Binder;
using System.Collections.Generic;

namespace SolidRpc.Abstractions.OpenApi.Proxy
{

    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public interface ISolidRpcOpenApiConfig : ISolidProxyInvocationAdviceConfig
    {
        /// <summary>
        /// The http headers to add to the invocations
        /// </summary>
        IDictionary<string, string> HttpHeaders { get; set; }

        /// <summary>
        /// Sets the open api specification to use. If not set the specification matching
        /// the method name or assembly name where the method is defined will be used.
        /// </summary>
        string OpenApiSpec { get; set; }

        /// <summary>
        /// The method to transform the Uri. This delegate is invoked to determine
        /// the base Uri for the service. Supplied uri is the one obtained from
        /// the openapi config.
        /// </summary>
        MethodAddressTransformer MethodAddressTransformer { get; set; }
    }
}
