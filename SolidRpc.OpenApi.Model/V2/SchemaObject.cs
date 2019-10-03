using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// The Schema Object allows the definition of input and output data types. These types can be objects, but also primitives and arrays. This object is based on the JSON Schema Specification Draft 4 and uses a predefined subset of it. On top of this subset, there are extensions provided by this specification to allow for more complete documentation.
    /// Further information about the properties can be found in JSON Schema Core and JSON Schema Validation.Unless stated otherwise, the property definitions follow the JSON Schema specification as referenced here.
    /// The following properties are taken directly from the JSON Schema definition and follow the same specifications:
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#schemaObject"/>
    public class SchemaObject : ItemsObject
    {
        public static SchemaObject CreateSchemaObject(ModelBase parent, Type type)
        {
            if (type == typeof(void) || type == typeof(Task))
            {
                // skip
            }
            else
            {
                if (type.IsTaskType(out Type taskType))
                {
                    type = taskType;
                }

            }
            var so = new SchemaObject(parent);
            return so;
        }

        /// <summary>
        /// Sets the type info based on supplied type
        /// </summary>
        /// <param name="itemBase"></param>
        /// <param name="type"></param>
        public static void SetTypeInfo(ItemBase itemBase, Type type)
        {
            if (type == typeof(bool))
            {
                itemBase.Type = "boolean";
                return;
            }
            else if (type == typeof(string))
            {
                itemBase.Type = "string";
                return;
            }
            else if (type == typeof(short))
            {
                itemBase.Type = "integer";
                itemBase.Format = "int16";
                return;
            }
            else if (type == typeof(int))
            {
                itemBase.Type = "integer";
                itemBase.Format = "int32";
                return;
            }
            else if (type == typeof(long))
            {
                itemBase.Type = "integer";
                itemBase.Format = "int64";
                return;
            }
            else if (type == typeof(float))
            {
                itemBase.Type = "number";
                itemBase.Format = "float";
                return;
            }
            else if (type == typeof(double))
            {
                itemBase.Type = "number";
                itemBase.Format = "double";
                return;
            }
            else if (type == typeof(System.IO.Stream))
            {
                itemBase.Type = "string";
                itemBase.Format = "binary";
                return;
            }
            else if (type == typeof(System.DateTime))
            {
                itemBase.Type = "string";
                itemBase.Format = "date";
                return;
            }
            else if (type == typeof(System.DateTimeOffset))
            {
                itemBase.Type = "string";
                itemBase.Format = "date-time";
                return;
            }
            else if (type == typeof(System.Uri))
            {
                itemBase.Type = "string";
                itemBase.Format = "uri";
                return;
            }
            else if (type == typeof(System.Guid))
            {
                itemBase.Type = "string";
                itemBase.Format = "uuid";
                return;
            }

            throw new NotImplementedException(type.FullName);
        }

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

        /// <summary>
        /// Specifies if this object is required
        /// </summary>
        [DataMember(Name = "required", EmitDefaultValue = false)]
        public IEnumerable<string> Required { get; set; }

        /// <summary>
        /// Returns true if this type is a file type
        /// </summary>
        /// <returns></returns>
        public bool IsBinaryType()
        {
            var schema = GetRefSchema() ?? this;
            if(schema.Type == "string" && schema.Format == "binary")
            {
                return true;
            }
            if (schema.Properties == null)
            {
                return false;
            }
            var props = schema.Properties.ToDictionary(o => o.Key, o => o.Value.GetClrType());
            return TypeExtensions.IsFileType(GetClrType().FullName, props);
        }
    }
}