using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    public class CSharpTypeExtends : CSharpMember, ICSharpTypeExtends
    {
        public CSharpTypeExtends(ICSharpMember parent, ICSharpMember extType) : base(parent, extType.FullName)
        {
        }

        public override void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.Emit(Name);
        }
    }
}
