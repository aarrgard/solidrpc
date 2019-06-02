using System.Runtime.Serialization;

namespace SolidRpc.Swagger.Model.V2
{
    /// <summary>
    /// Allows adding meta data to a single tag that is used by the Operation Object. It is not mandatory to have a Tag Object per tag used there.
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#tagObject"/>
    public class TagObject : ModelBase
    {
        public TagObject(ModelBase parent) : base(parent)
        {

        }
        /// <summary>
        /// Required. The name of the tag.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Additional external documentation for this tag.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Additional external documentation for this tag.
        /// </summary>
        [DataMember(Name = "externalDocs", EmitDefaultValue = false)]
        public ExternalDocumentationObject ExternalDocs { get; set; }
    }
}