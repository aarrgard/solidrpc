using System.Linq;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    public class CSharpNamespace : CSharpMember, ICSharpNamespace
    {

        public CSharpNamespace(ICSharpMember parent, string name) : base(parent, name)
        {
        }

        public override void WriteCode(ICodeWriter codeWriter)
        {
            Members.ToList().ForEach(o => o.WriteCode(codeWriter));
        }
    }
}