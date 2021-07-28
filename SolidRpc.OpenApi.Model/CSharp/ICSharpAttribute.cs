using System.Collections.Generic;

namespace SolidRpc.OpenApi.Model.CSharp
{
    /// <summary>
    /// Represents an attribute.
    /// </summary>
    public interface ICSharpAttribute : ICSharpMember
    {
        /// <summary>
        /// The arguments
        /// </summary>
        IEnumerable<object> Args { get; }

        /// <summary>
        /// The attribute properties
        /// </summary>
        IDictionary<string, object> AttributeData { get; }
    }
}
