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
        /// Returns the spec resolver used to find this spec.
        /// </summary>
        IOpenApiSpecResolver OpenApiSpecResolver { get; }

        /// <summary>
        /// Returns the address in the resolver where we can find this spec
        /// </summary>
        string OpenApiSpecResolverAddress { get; }

        /// <summary>
        /// Sets the openapi resolver and the address of this specification.
        /// </summary>
        /// <param name="openApiSpecResolver"></param>
        /// <param name="openApiSpecResolverAddress"></param>
        void SetOpenApiSpecResolver(IOpenApiSpecResolver openApiSpecResolver, string openApiSpecResolverAddress);

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
        /// If the root address matches this spec then this spec is returned. Otherwise
        /// this spec is clone:ed and updated.
        /// </summary>
        /// <param name="rootAddress"></param>
        IOpenApiSpec SetBaseAddress(Uri rootAddress);

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

        /// <summary>
        /// Removes all the relative paths in the refs. ie. a reference
        /// to ../../test.json#/definitions/x will become test.json#/definitions/x
        /// </summary>
        void RemoveRelativeRefPaths();
    }
}