using SolidProxy.Core.Configuration;
using SolidRpc.Abstractions.OpenApi.Binder;
using System;
using System.Collections.Generic;

namespace SolidRpc.Abstractions.OpenApi.Proxy
{

    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public interface ISolidRpcOpenApiConfig : ISolidProxyInvocationAdviceConfig
    {
        /// <summary>
        /// Sets the open api specification to use. If not set the specification matching
        /// the method name or assembly name where the method is defined will be used.
        /// </summary>
        string OpenApiSpec { get; set; }

        /// <summary>
        /// If this key is set it needs to be specified in the invocation
        /// properties in order for the invocation to be authorized on the server side.
        /// If the key is present on the client side it is added to call so that
        /// the invocation is authorized.
        /// </summary>
        KeyValuePair<string, string>? SecurityKey { get; set; }

        /// <summary>
        /// The method to transform the Uri. This delegate is invoked to determine
        /// the base Uri for the service. Supplied uri is the one obtained from
        /// the openapi config.
        /// </summary>
        MethodAddressTransformer MethodAddressTransformer { get; set; }

        /// <summary>
        /// The http headers to add to the invocations
        /// </summary>
        MethodHeadersTransformer MethodHeadersTransformer { get; set; }
    }
}
