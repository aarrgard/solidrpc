using System.Runtime.Serialization;

namespace SolidRpc.Swagger.Model.V2
{
    /// <summary>
    /// Allows adding meta data to a single tag that is used by the Operation Object. It is not mandatory to have a Tag Object per tag used there.
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#externalDocumentationObject"/>
    public class ExternalDocumentationObject : ModelBase
    {
        public ExternalDocumentationObject(ModelBase parent) : base(parent) { }
        /// <summary>
        /// Required. The URL for the target documentation. Value MUST be in the format of a URL.
        /// </summary>
        [DataMember(Name = "url", EmitDefaultValue = false)]
        public string Url { get; set; }

        /// <summary>
        /// A short description of the target documentation. GFM syntax can be used for rich text representation.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }
    }
}