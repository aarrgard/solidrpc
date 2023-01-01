using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.CSharp
{
    /// <summary>
    /// Represents a c# field
    /// </summary>
    public interface ICSharpField : ICSharpMember
    {
        /// <summary>
        /// The type that the field represents
        /// </summary>
        ICSharpType FieldType { get; }

        /// <summary>
        /// The value of the field
        /// </summary>
        string Value { get; }
    }
}
