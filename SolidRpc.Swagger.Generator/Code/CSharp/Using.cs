using System;
using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class Using : Member, IUsing
    {
        public Using(IMember parent, string ns) : base(parent)
        {
            Name = ns;
        }

        public override string Name { get; }

        public override void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.Emit($"using {Name};{codeWriter.NewLine}");
        }
    }
}
