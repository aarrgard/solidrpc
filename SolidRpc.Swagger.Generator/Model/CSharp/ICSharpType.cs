using System;
using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Model.CSharp
{
    /// <summary>
    /// Represents a C# type - class, interface or primitive
    /// </summary>
    public interface ICSharpType : ICSharpMember
    {
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
        /// Returns the enumerable type
        /// </summary>
        ICSharpType EnumerableType { get; }

        /// <summary>
        /// Returns the task type - null if this is not a task.
        /// </summary>
        ICSharpType TaskType { get; }
    }
}