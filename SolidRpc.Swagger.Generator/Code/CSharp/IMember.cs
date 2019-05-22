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
        /// The parent member
        /// </summary>
        IMember Parent { get; }

        /// <summary>
        /// Returns the parent of supplied type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetParent<T>() where T:IMember;

        /// <summary>
        /// Returns all the members.
        /// </summary>
        IList<IMember> Members { get; }

        /// <summary>
        /// Writes the code to supplied writer
        /// </summary>
        /// <param name="codeWriter"></param>
        void WriteCode(ICodeWriter codeWriter);
    }
}