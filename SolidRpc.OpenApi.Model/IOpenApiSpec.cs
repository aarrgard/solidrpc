using System;

namespace SolidRpc.OpenApi.Model
{
    /// <summary>
    /// Interface implemented by the SwaggerObject structures in the model.
    /// </summary>
    public interface IOpenApiSpec
    {
        /// <summary>
        /// Updates the spec so that the host and port are from the supplied address.
        /// </summary>
        /// <param name="rootAddress"></param>
        void SetSchemeAndHostAndPort(Uri rootAddress);

        /// <summary>
        /// Writes this spec as a json string
        /// </summary>
        /// <returns></returns>
        string WriteAsJsonString();
    }
}