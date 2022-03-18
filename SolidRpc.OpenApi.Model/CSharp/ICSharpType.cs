using System;
using System.Collections.Generic;

namespace SolidRpc.OpenApi.Model.CSharp
{
    /// <summary>
    /// Represents a C# type - class, interface or primitive
    /// </summary>
    public interface ICSharpType : ICSharpMember
    {
        /// <summary>
        /// Specifies if this type has been inititalized
        /// </summary>
        bool Initialized { get; set; }

        /// <summary>
        /// The namepspace that this member belongs to.
        /// </summary>
        ICSharpNamespace Namespace { get; }

        /// <summary>
        /// Returns true if this type is a runtime type. 
        /// </summary>
        Type RuntimeType { get; }

        /// <summary>
        /// The methods in this type
        /// </summary>
        IEnumerable<ICSharpMethod> Methods { get; }

        /// <summary>
        /// The properties in this type
        /// </summary>
        IEnumerable<ICSharpProperty> Properties { get; }

        /// <summary>
        /// Adds an extension to this type.
        /// </summary>
        /// <param name="extType"></param>
        void AddExtends(ICSharpType extType);

        /// <summary>
        /// Returns the enumerable type
        /// </summary>
        ICSharpType EnumerableType { get; }

        /// <summary>
        /// Returns the task type - null if this is not a task.
        /// </summary>
        ICSharpType TaskType { get; }

        /// <summary>
        /// Returns the nullable type - null if this is not a task.
        /// </summary>
        ICSharpType NullableType { get; }

        /// <summary>
        /// Returns true if this is a value type
        /// </summary>
        bool IsEnumType { get; }

        /// <summary>
        /// Returns true if this is an enum type
        /// </summary>
        bool IsValueType { get; }

        /// <summary>
        /// Returns true if this is a file type.
        /// </summary>
        bool IsFileType { get; }

        /// <summary>
        /// Returns true if this is a generic type
        /// </summary>
        bool IsGenericType { get; }

        /// <summary>
        /// Returns the generic arguments - null if type is not a generic type.
        /// </summary>
        /// <returns></returns>
        ICSharpType GetGenericType();

        /// <summary>
        /// Returns the generic arguments - null if type is not a generic type.
        /// </summary>
        /// <returns></returns>
        ICollection<ICSharpType> GetGenericArguments();

        /// <summary>
        /// Returns true if the type is a dictionary type.
        /// </summary>
        /// <param name="keyType"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        bool IsDictionaryType(out ICSharpType keyType, out ICSharpType valueType);
    }
}