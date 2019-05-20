using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    /// <summary>
    /// Base interface for all members
    /// </summary>
    public interface IMember
    {
        /// <summary>
        /// Returns all the members.
        /// </summary>
        IEnumerable<IMember> Members { get; }
    }
}