using System.Collections.Generic;

namespace SolidRpc.OpenApi.Model.CSharp
{
    /// <summary>
    /// The base type for all the members
    /// </summary>
    public interface ICSharpMember
    {

        /// <summary>
        /// The parent member.
        /// </summary>
        ICSharpMember Parent { get; }

        /// <summary>
        /// Returns the parent of supplied type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetParent<T>() where T : ICSharpMember;

        /// <summary>
        /// The members that this member contains.
        /// </summary>
        IEnumerable<ICSharpMember> Members { get; }

        /// <summary>
        /// The name of the member
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the full name for this member.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// The comment for this member.
        /// </summary>
        ICSharpComment Comment { get; }

        /// <summary>
        /// Parses the supplied code comment
        /// </summary>
        /// <param name="comment"></param>
        void ParseComment(string comment);

        /// <summary>
        /// Adds a member to this member.
        /// </summary>
        /// <param name="member"></param>
        void AddMember(ICSharpMember member);

        /// <summary>
        /// Writes the code to supplied writer
        /// </summary>
        /// <param name="codeWriter"></param>
        void WriteCode(ICodeWriter codeWriter);

        /// <summary>
        /// Returns all the namespaces in this member.
        /// </summary>
        /// <param name="namespaces"></param>
        void GetNamespaces(IDictionary<string, HashSet<string>> namespaces);
    }
}
