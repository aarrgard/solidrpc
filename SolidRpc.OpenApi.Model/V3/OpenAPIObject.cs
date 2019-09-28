using SolidRpc.Abstractions.OpenApi.Model;
using System;
using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Model.V3
{
    /// <summary>
    /// This is the root document object of the OpenAPI document.
    /// </summary>
    /// <see cref="https://swagger.io/specification/#oasObject"/>
    public class OpenAPIObject : ModelBase, IOpenApiSpec
    {
        public OpenAPIObject(IOpenApiSpecResolver openApiSpecResolver) : base(null)
        {
            OpenApiSpecResolver = openApiSpecResolver;
        }
        /// <summary>
        /// REQUIRED. This string MUST be the semantic version number of the OpenAPI Specification version that the OpenAPI document uses. The openapi field SHOULD be used by tooling specifications and clients to interpret the OpenAPI document. This is not related to the API info.version string.
        /// </summary>
        [DataMember(Name = "swagger", IsRequired = true, EmitDefaultValue = false)]
        public string Openapi { get; set; }

        public string OpenApiVersion => "3.0";

        public Uri BaseAddress => new Uri("http://localhost");

        public string Title => "Unknown";

        public IOpenApiSpecResolver OpenApiSpecResolver { get; set; }

        public string OpenApiSpecResolverAddress => throw new NotImplementedException();

        public IOpenApiSpec Clone()
        {
            throw new NotImplementedException();
        }

        public IOpenApiSpec SetBaseAddress(Uri baseAddress)
        {
            throw new NotImplementedException();
        }

        public void SetExternalDoc(string description, Uri indexHtmlPath)
        {
            throw new NotImplementedException();
        }

        public void SetOpenApiSpecResolver(IOpenApiSpecResolver openApiSpecResolver, string address)
        {
            throw new NotImplementedException();
        }

        public string WriteAsJsonString(bool formatted = false)
        {
            throw new NotImplementedException();
        }
    }
}
