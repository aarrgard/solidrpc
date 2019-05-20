using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    /// <summary>
    /// Base interface for all members
    /// </summary>
    public interface IMember
    {
        /// <summary>
        /// The name of the member
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns all the members.
        /// </summary>
        IEnumerable<IMember> Members { get; }

        /// <summary>
        /// Writes the code to supplied writer
        /// </summary>
        /// <param name="codeWriter"></param>
        void WriteCode(ICodeWriter codeWriter);
    }
}