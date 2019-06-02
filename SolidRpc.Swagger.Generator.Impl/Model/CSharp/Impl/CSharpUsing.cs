using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
{
    public class CSharpUsing : CSharpMember, ICSharpUsing
    {
        public CSharpUsing(ICSharpMember parent, string name) : base(parent, name)
        {
        }

        public override void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.Emit($"using {Name};{codeWriter.NewLine}");
        }
    }
}
