using System.Collections.Generic;

namespace SolidRpc.OpenApi.Model.CSharp
{
    /// <summary>
    /// Represents a c# namespace.
    /// </summary>
    public interface ICSharpNamespace : ICSharpMember
    {
        /// <summary>
        /// Returns all the namespace parts. ie namespace.split('.')
        /// </summary>
        IEnumerable<string> Parts { get; }

        /// <summary>
        /// Returns all the namespaces that this namespace refers to.
        /// Including the FullName of this namespace and the empty("") namespace
        /// </summary>
        IEnumerable<string> Namespaces { get; }
    }
}
