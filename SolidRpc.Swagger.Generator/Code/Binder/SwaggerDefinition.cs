using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.Binder
{
    /// <summary>
    /// Represents a swagger object.
    /// </summary>
    public class SwaggerDefinition
    {
        public static string TypeVoid = "void";
        public static string TypeBoolean = "bool";
        public static string TypeString = "string";
        public static string TypeInt = "int";
        public static string TypeLong = "long";
        public static string TypeFloat = "float";
        public static string TypeDouble = "double";
        public static string TypeStream = "System.IO.Stream";
        public static string TypeDateTime = "System.DateTime";
        public static string TypeGuid = "System.Guid";
        public static string TypeTask = "System.Threading.Tasks.Task";
        public static string TypeCancellationToken = "System.Threading.CancellationToken";

        public static IEnumerable<string> ReservedNames = new string[]
        {
            TypeVoid,
            TypeBoolean,
            TypeString,
            TypeInt,
            TypeLong,
            TypeFloat,
            TypeDouble,
            TypeStream,
            TypeDateTime,
            TypeGuid,
            TypeTask,
            TypeCancellationToken
        };

        public SwaggerDefinition(SwaggerOperation swaggerOperation, string name)
        {
            SwaggerOperation = swaggerOperation;
            Name = name ?? throw new ArgumentNullException();
            IsReservedName = ReservedNames.Any(o => o == Name);
            Properties = new List<SwaggerProperty>();
        }

        public SwaggerDefinition(SwaggerDefinition arrayType) : this(arrayType.SwaggerOperation, arrayType.Name)
        {
            ArrayType = arrayType;
        }

        /// <summary>
        /// The swagger operation that this definition belongs to. If the 
        /// definition is global then this property is null.
        /// </summary>
        public SwaggerOperation SwaggerOperation { get; }

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns true if the name is reserved.
        /// </summary>
        public bool IsReservedName { get; }

        /// <summary>
        /// Specifies if this definition represents an array of objects.
        /// </summary>
        public SwaggerDefinition ArrayType { get; }

        /// <summary>
        /// The properties that belongs to this definition.
        /// </summary>
        public IEnumerable<SwaggerProperty> Properties { get; set; }

        /// <summary>
        /// The additional properties
        /// </summary>
        public SwaggerDefinition AdditionalProperties { get; set; }

        /// <summary>
        /// The description of this definition
        /// </summary>
        public string Description { get; internal set; }
    }
}
