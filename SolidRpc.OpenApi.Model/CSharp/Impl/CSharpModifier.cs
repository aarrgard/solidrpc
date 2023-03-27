using System;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Represents a csharp enum
    /// </summary>
    public class CSharpModifier : CSharpMember, ICSharpModifier
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        public CSharpModifier(ICSharpMember parent, string name) : base(parent, name)
        {
        }

        /// <summary>
        /// Emits the constructor to supplied writer.
        /// </summary>
        /// <param name="codeWriter"></param>
        public override void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.Emit($"{Name} ");
        }
    }
}