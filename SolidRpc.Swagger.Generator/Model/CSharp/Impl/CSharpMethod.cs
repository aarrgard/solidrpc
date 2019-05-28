using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
{
    public class CSharpMethod : CSharpMember, ICSharpMethod
    {
        public CSharpMethod(ICSharpMember parent, string name) : base(parent, name)
        {
        }

        public ICSharpType ReturnType { get; set; }

        public IEnumerable<ICSharpMethodParameter> Parameters { get; set; }
    }
}