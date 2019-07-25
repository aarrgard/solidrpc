using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// Describes a single API operation on a path.
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#parameterObject"/>
    public class ParameterObject : ItemBase
    {
        public static IEnumerable<ParameterObject> EmptyList = new ParameterObject[0];

        public ParameterObject(ModelBase parent) : base(parent) { }

        /// <summary>
        /// Required. The name of the parameter. Parameter names are case sensitive.
        /// If in is "path", the name field MUST correspond to the associated path segment from the path field in the Paths Object.See Path Templating for further information.
        /// For all other cases, the name corresponds to the parameter name used based on the in property.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Required. The location of the parameter. Possible values are "query", "header", "path", "formData" or "body".
        /// </summary>
        [DataMember(Name = "in", EmitDefaultValue = false)]
        public string In { get; set; }

        /// <summary>
        /// Determines whether this parameter is mandatory. If the parameter is in "path", this property is required and its value MUST be true. Otherwise, the property MAY be included and its default value is false.
        /// </summary>
        [DataMember(Name = "required")]
        public bool Required { get; set; }

        /// <summary>
        /// Required. The schema defining the type used for the body parameter.
        /// </summary>
        [DataMember(Name = "schema", EmitDefaultValue = false)]
        public SchemaObject Schema { get; set; }

        /// <summary>
        /// Returns true if this parameter is a body type
        /// </summary>
        /// <returns></returns>
        public bool IsBodyType()
        {
            return string.Equals(In,"body",StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Returns true if this parameter is a file type
        /// </summary>
        /// <returns></returns>
        public bool IsBinaryType()
        {
            if (Type == "file") return true;
            if (Type == "string" && Format == "binary") return true;
            if (Schema == null) return false;
            return Schema.IsBinaryType(); 
        }

        /// <summary>
        /// Returns the name of the file parameter
        /// </summary>
        /// <returns></returns>
        public string GetFileParameterName()
        {
            var fileTypes = GetParent<OperationObject>().Parameters.Where(o => o.Type == "file").FirstOrDefault();
            return fileTypes?.Name;
        }

        /// <summary>
        /// Sets the type info
        /// </summary>
        /// <param name="parameterType"></param>
        public void SetTypeInfo(Type parameterType)
        {
            In = "query";
            Schema = SchemaObject.CreateSchemaObject(this, parameterType);
        }
    }
}