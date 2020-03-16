using System.Collections.Generic;

namespace SolidRpc.OpenApi.Model.CSharp
{
    /// <summary>
    /// Represents a c# enum.
    /// </summary>
    public interface ICSharpEnum : ICSharpType
    {
        /// <summary>
        /// The enum values
        /// </summary>
        IEnumerable<ICSharpEnumValue> EnumValues { get; }
    }
}