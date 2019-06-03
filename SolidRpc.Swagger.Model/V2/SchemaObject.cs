using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SolidRpc.Swagger.Model.V2
{
    /// <summary>
    /// he Schema Object allows the definition of input and output data types. These types can be objects, but also primitives and arrays. This object is based on the JSON Schema Specification Draft 4 and uses a predefined subset of it. On top of this subset, there are extensions provided by this specification to allow for more complete documentation.
    /// Further information about the properties can be found in JSON Schema Core and JSON Schema Validation.Unless stated otherwise, the property definitions follow the JSON Schema specification as referenced here.
    /// The following properties are taken directly from the JSON Schema definition and follow the same specifications:
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#schemaObject"/>
    public class SchemaObject : ItemsObject
    {
        public SchemaObject(ModelBase parent) : base(parent) { }

        [DataMember(Name = "discriminator", EmitDefaultValue = false)]
        public string Discriminator { get; set; }

        [DataMember(Name = "readOnly", EmitDefaultValue = false)]
        public bool ReadOnly { get; set; }

        [DataMember(Name = "xml", EmitDefaultValue = false)]
        public XmlObject Xml { get; set; }

        [DataMember(Name = "externalDocs", EmitDefaultValue = false)]
        public ExternalDocumentationObject ExternalDocs { get; set; }

        [DataMember(Name = "example", EmitDefaultValue = false)]
        public object Example { get; set; }

        [DataMember(Name = "properties", EmitDefaultValue = false)]
        public DefinitionsObject Properties { get; set; }

        [DataMember(Name = "additionalProperties", EmitDefaultValue = false)]
        public SchemaObject AdditionalProperties { get; set; }

        [DataMember(Name = "required", EmitDefaultValue = false)]
        public IEnumerable<string> Required { get; set; }
    }
}