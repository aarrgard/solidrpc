using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SolidRpc.Swagger.Model.V2
{
    /// <summary>
    /// Describes a single response from an API Operation.
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#itemsObject"/>
    public class ItemBase : ModelBase
    {
        public ItemBase(ModelBase parent) : base(parent) { }
        [DataMember(Name = "$ref", EmitDefaultValue = false)]
        public string Ref { get; set; }

        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }

        /// <summary>
        /// The extending format for the previously mentioned type. See Data Type Formats for further details.
        /// </summary>
        [DataMember(Name = "format", EmitDefaultValue = false)]
        public string Format { get; set; }

        /// <summary>
        /// Describes this item.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Required if type is "array". Describes the type of items in the array.
        /// </summary>
        [DataMember(Name = "items", EmitDefaultValue = false)]
        public ItemsObject Items { get; set; }

        /// <summary>
        /// Determines the format of the array if type array is used. Possible values are:
        /// csv - comma separated values foo, bar.
        /// ssv - space separated values foo bar.
        /// tsv - tab separated values foo\tbar.
        /// pipes - pipe separated values foo|bar.
        /// Default value is csv.
        /// </summary>
        [DataMember(Name = "collectionFormat", EmitDefaultValue = false)]
        public string CollectionFormat { get; set; }

        /// <summary>
        /// Declares the value of the item that the server will use if none is provided. (Note: "default" has no meaning for required items.) See https://tools.ietf.org/html/draft-fge-json-schema-validation-00#section-6.2. Unlike JSON Schema this value MUST conform to the defined type for the data type.
        /// </summary>
        [DataMember(Name = "default", EmitDefaultValue = false)]
        public object Default { get; set; }

        [DataMember(Name = "maximum", EmitDefaultValue = false)]
        public decimal Maximum { get; set; }

        [DataMember(Name = "exclusiveMaximum", EmitDefaultValue = false)]
        public bool ExclusiveMaximum { get; set; }

        [DataMember(Name = "minimum", EmitDefaultValue = false)]
        public decimal Minimum { get; set; }

        [DataMember(Name = "exclusiveMinimum", EmitDefaultValue = false)]
        public bool ExclusiveMinimum { get; set; }

        [DataMember(Name = "maxLength", EmitDefaultValue = false)]
        public int MaxLength { get; set; }

        [DataMember(Name = "minLength", EmitDefaultValue = false)]
        public int MinLength { get; set; }

        [DataMember(Name = "pattern", EmitDefaultValue = false)]
        public string Pattern { get; set; }

        [DataMember(Name = "maxItems", EmitDefaultValue = false)]
        public int MaxItems { get; set; }

        [DataMember(Name = "minItems", EmitDefaultValue = false)]
        public int MinItems { get; set; }

        [DataMember(Name = "uniqueItems", EmitDefaultValue = false)]
        public bool UniqueItems { get; set; }

        [DataMember(Name = "enum", EmitDefaultValue = false)]
        public IEnumerable<string> Enum { get; set; }

        [DataMember(Name = "multipleOf", EmitDefaultValue = false)]
        public decimal MultipleOf { get; set; }

        public string OperationName
        {
            get
            {
                if (Parent is DefinitionsObject defObj)
                {
                    return defObj.Where(o => ReferenceEquals(o.Value, this)).First().Key;
                }
                if (Parent is ResponseObject respObj)
                {
                    return respObj.Status;
                }
                if (Parent is OperationObject opObj)
                {
                    return opObj.OperationId;
                }
                else
                {
                    throw new Exception("Cannot handle object type:" + Parent?.GetType().FullName);
                }
            }
        }
    }
}