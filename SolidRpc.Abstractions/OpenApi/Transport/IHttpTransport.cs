using SolidRpc.Abstractions.OpenApi.Http;
using System;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Transport
{
    /// <summary>
    /// Represents the settings for the http transport
    /// </summary>
    public interface IHttpTransport : ITransport
    {
        /// <summary>
        /// The base address of the method.
        /// </summary>
        Uri BaseAddress { get; set; }

        /// <summary>
        /// The path relative to the base method.
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// The method to transform the Uri. This delegate is invoked to determine
        /// the base Uri for the service. Supplied uri is the one obtained from
        /// the openapi config.
        /// </summary>
        MethodAddressTransformer MethodAddressTransformer { get; set; }
    }
}