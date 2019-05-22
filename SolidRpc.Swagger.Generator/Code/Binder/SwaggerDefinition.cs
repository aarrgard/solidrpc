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
        public static string TypeStream = "System.IO.Stream";
        public static string TypeDateTime = "System.DateTime";
        public static string TypeTask = "System.Threading.Tasks.Task";
        public static string TypeCancellationToken = "System.Threading.CancellationToken";

        public static IEnumerable<string> ReservedNames = new string[]
        {
            TypeVoid,
            TypeBoolean,
            TypeString,
            TypeInt,
            TypeLong,
            TypeStream,
            TypeDateTime,
            TypeTask,
            TypeCancellationToken
        };

        public static SwaggerDefinition Void = new SwaggerDefinition(null, TypeVoid);

        public SwaggerDefinition(SwaggerOperation swaggerOperation, string name)
        {
            SwaggerOperation = swaggerOperation;
            Name = name ?? throw new ArgumentNullException();
            IsReservedName = ReservedNames.Any(o => o == Name);
            Properties = new List<SwaggerProperty>();
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
        public bool IsArray { get; set; }

        /// <summary>
        /// The properties that belongs to this definition.
        /// </summary>
        public IEnumerable<SwaggerProperty> Properties { get; set; }
    }
}
