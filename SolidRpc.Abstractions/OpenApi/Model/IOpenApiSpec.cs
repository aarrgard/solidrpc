using System;

namespace SolidRpc.Abstractions.OpenApi.Model
{
    /// <summary>
    /// Interface implemented by the SwaggerObject structures in the model.
    /// </summary>
    public interface IOpenApiSpec
    {
        /// <summary>
        /// Clones this open api spec
        /// </summary>
        /// <returns></returns>
        IOpenApiSpec Clone();

        /// <summary>
        /// The openapi version of this specification
        /// </summary>
        string OpenApiVersion { get; }

        /// <summary>
        /// Returns the title of the spec.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Returns the base address. This is the combination of
        /// Scheme, Host, Port and BasePath.
        /// </summary>
        Uri BaseAddress { get; }

        /// <summary>
        /// Updates the spec so that the host and port are from the supplied address.
        /// </summary>
        /// <param name="rootAddress"></param>
        void SetBaseAddress(Uri rootAddress);

        /// <summary>
        /// Writes this spec as a json string
        /// </summary>
        /// <param name="formatted"></param>
        /// <returns></returns>
        string WriteAsJsonString(bool formatted = false);

        /// <summary>
        /// Sets the associated external documentation.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="indexHtmlPath"></param>
        void SetExternalDoc(string description, Uri indexHtmlPath);
    }
}