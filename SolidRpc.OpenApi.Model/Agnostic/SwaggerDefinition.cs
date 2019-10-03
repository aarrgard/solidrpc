using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.Model.Agnostic
{
    /// <summary>
    /// Represents a swagger object.
    /// </summary>
    public class SwaggerDefinition
    {
        /// <summary>
        /// The void type
        /// </summary>
        public static string TypeVoid = "void";
        /// <summary>
        /// The bool type
        /// </summary>
        public static string TypeBoolean = "bool";
        /// <summary>
        /// The string type
        /// </summary>
        public static string TypeString = "string";
        /// <summary>
        /// The int type
        /// </summary>
        public static string TypeInt = "int";
        /// <summary>
        /// The long type
        /// </summary>
        public static string TypeLong = "long";
        /// <summary>
        /// The float type
        /// </summary>
        public static string TypeFloat = "float";
        /// <summary>
        /// The double type
        /// </summary>
        public static string TypeDouble = "double";
        /// <summary>
        /// The stream type
        /// </summary>
        public static string TypeStream = "System.IO.Stream";
        /// <summary>
        /// The date time type
        /// </summary>
        public static string TypeDateTime = "System.DateTime";
        /// <summary>
        /// The date time type
        /// </summary>
        public static string TypeDateTimeOffset = "System.DateTimeOffset";
        /// <summary>
        /// The guid type
        /// </summary>
        public static string TypeGuid = "System.Guid";
        /// <summary>
        /// The uri type
        /// </summary>
        public static string TypeUri = "System.Uri";
        /// <summary>
        /// The task type
        /// </summary>
        public static string TypeTask = "System.Threading.Tasks.Task";
        /// <summary>
        /// The cancellation token type.
        /// </summary>
        public static string TypeCancellationToken = "System.Threading.CancellationToken";

        /// <summary>
        /// The reserved names.
        /// </summary>
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
            TypeDateTimeOffset,
            TypeGuid,
            TypeUri,
            TypeTask,
            TypeCancellationToken
        };

        /// <summary>
        /// Constrcuts a new instance
        /// </summary>
        /// <param name="swaggerOperation"></param>
        /// <param name="name"></param>
        public SwaggerDefinition(SwaggerOperation swaggerOperation, string name)
        {
            SwaggerOperation = swaggerOperation;
            Name = name ?? throw new ArgumentNullException();
            IsReservedName = ReservedNames.Any(o => o == Name);
            Properties = new List<SwaggerProperty>();
        }

        /// <summary>
        /// Constrcuts a new instance
        /// </summary>
        /// <param name="arrayType"></param>
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
        public string Description { get; set; }

        /// <summary>
        /// The exception code if this is a type that should return a result.
        /// </summary>
        public int? ExceptionCode { get; set; }
    }
}
