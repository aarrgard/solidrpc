using System;

namespace SolidRpc.Abstractions.OpenApi.Transport
{
    /// <summary>
    /// Represents the settings for the queue transport
    /// </summary>
    public interface IHttpTransport : ITransport
    {
        /// <summary>
        /// The method to transform the Uri. This delegate is invoked to determine
        /// the base Uri for the service. Supplied uri is the one obtained from
        /// the openapi config.
        /// </summary>
        MethodAddressTransformer MethodAddressTransformer { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodAddressTransformer"></param>
        /// <returns></returns>
        IHttpTransport SetMethodAddressTransformer(MethodAddressTransformer methodAddressTransformer);

        /// <summary>
        /// The http headers to add to the invocations
        /// </summary>
        MethodHeadersTransformer MethodHeadersTransformer { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodHeadersTransformer"></param>
        /// <returns></returns>
        IHttpTransport SetMethodHeadersTransformer(MethodHeadersTransformer methodHeadersTransformer);
    }
}