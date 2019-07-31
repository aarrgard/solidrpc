using System;

namespace SolidRpc.Abstractions.OpenApi.Model
{
    /// <summary>
    /// Interface implemented by the SwaggerObject structures in the model.
    /// </summary>
    public interface IOpenApiSpec
    {

        /// <summary>
        /// The openapi version of this specification
        /// </summary>
        string OpenApiVersion { get; }

        /// <summary>
        /// Returns the base path.
        /// </summary>
        Uri BaseAddress { get; }

        /// <summary>
        /// Updates the spec so that the host and port are from the supplied address.
        /// </summary>
        /// <param name="rootAddress"></param>
        void SetSchemeAndHostAndPort(Uri rootAddress);

        /// <summary>
        /// Writes this spec as a json string
        /// </summary>
        /// <param name="formatted"></param>
        /// <returns></returns>
        string WriteAsJsonString(bool formatted = false);
    }
}