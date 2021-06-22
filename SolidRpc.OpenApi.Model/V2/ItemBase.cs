using SolidRpc.Abstractions.OpenApi.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// Describes a single response from an API Operation.
    /// </summary>
    /// <a href="https://swagger.io/specification/v2/#itemsObject"/>
    public class ItemBase : ModelBase
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
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

        /// <summary>
        /// Return the referenced schema object if reference is set.
        /// </summary>
        /// <returns></returns>
        public SchemaObject GetRefSchema()
        {
            if (string.IsNullOrEmpty(Ref))
            {
                return null;
            }
            var hashIdx = Ref.IndexOf('#');
            if(hashIdx == -1)
            {
                throw new Exception($"Reference({Ref}) does not contain a hash");
            }
            var filePath = Ref.Substring(0, hashIdx);
            var refSource = GetParent<SwaggerObject>();
            if (!string.IsNullOrEmpty(filePath))
            {
                // reference to other file

                if(refSource.OpenApiSpecResolver.TryResolveApiSpec(filePath, out IOpenApiSpec refSpec))
                {
                    refSource = (SwaggerObject) refSpec;
                }
                else
                {
                    throw new Exception($"Could not find referenced file: {filePath}");
                }
            }
            var nodePath = Ref.Substring(hashIdx + 1);
            if (!nodePath.StartsWith("/definitions/"))
            {
                throw new Exception("Cannot find ref:" + Ref);
            }
            var key = nodePath.Substring("/definitions/".Length);
            return refSource.Definitions[key];
        }

        /// <summary>
        /// Returns the clr type that this item represents.
        /// </summary>
        /// <returns></returns>
        public Type GetClrType()
        {
            var item = GetRefSchema() ?? this;
            switch(item.Type)
            {
                case null:
                case "":
                case "object":
                case "file":
                    return typeof(object);
                case "array":
                    var arrayType = Items?.GetClrType();
                    if(arrayType == null || arrayType == typeof(object))
                    {
                        return typeof(object);
                    }
                    return arrayType.MakeArrayType();
                case "boolean":
                    return typeof(bool);
                case "number":
                case "integer":
                    switch (item.Format)
                    {
                        case "double":
                            return typeof(double);
                        case "byte":
                            return typeof(byte);
                        case "float":
                            return typeof(float);
                        case "int32":
                            return typeof(int);
                        case "int64":
                            return typeof(long);
                        case "decimal":
                            return typeof(decimal);
                    }
                    break;
                case "string":
                    switch (Format)
                    {
                        case null:
                        case "":
                            return typeof(string);
                        case "byte":
                        case "binary":
                            return typeof(Stream);
                        case "date":
                            return typeof(DateTime);
                        case "date-time":
                            return typeof(DateTimeOffset);
                        case "time-span":
                            return typeof(TimeSpan);
                        case "uuid":
                            return typeof(Guid);
                        case "uri":
                            return typeof(Uri);
                    }
                    break;
            }
            throw new Exception($"Cannot determine type for {item.Type}/{item.Format}");
        }

        public string GetBaseType()
        {
            var refSchema = GetRefSchema();
            if(refSchema != null)
            {
                return refSchema.GetBaseType();
            }
            if (Type == "array")
            {
                return Items.GetBaseType();
            }
            return Type;
        }

        public string GetOperationName()
        {
            if (Parent is DefinitionsObject defObj)
            {
                return defObj.Where(o => ReferenceEquals(o.Value, this)).First().Key;
            }
            if (Parent is ResponseObject respObj)
            {
                return $"Response{respObj.GetStatus()}";
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