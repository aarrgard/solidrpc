using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// Allows adding meta data to a single tag that is used by the Operation Object. It is not mandatory to have a Tag Object per tag used there.
    /// <a href="https://swagger.io/specification/v2/#externalDocumentationObject"/>
    /// </summary>
    public class ExternalDocumentationObject : ModelBase
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
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