using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SolidRpc.Swagger.Model.V3
{
    /// <summary>
    /// This is the root document object of the OpenAPI document.
    /// </summary>
    /// <see cref="https://swagger.io/specification/#oasObject"/>
    public class OpenAPIObject : ModelBase, ISwaggerSpec
    {
        /// <summary>
        /// REQUIRED. This string MUST be the semantic version number of the OpenAPI Specification version that the OpenAPI document uses. The openapi field SHOULD be used by tooling specifications and clients to interpret the OpenAPI document. This is not related to the API info.version string.
        /// </summary>
        [DataMember(Name = "swagger", IsRequired = true, EmitDefaultValue = false)]
        public string Openapi { get; set; }
    }
}
