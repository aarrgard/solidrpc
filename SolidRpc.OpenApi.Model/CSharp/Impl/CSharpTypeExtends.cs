using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Represents an extends clause
    /// </summary>
    public class CSharpTypeExtends : CSharpMember, ICSharpTypeExtends
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="extType"></param>
        public CSharpTypeExtends(ICSharpMember parent, ICSharpType extType) : base(parent, extType.FullName)
        {
        }

        /// <summary>
        /// Writes the extends clause
        /// </summary>
        /// <param name="codeWriter"></param>
        public override void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.Emit(Name);
        }
    }
}
