using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class Interface : ClassOrInterface, IInterface
    {
        public Interface(Namespace ns, string name) : base(ns)
        {
            Name = name;
        }

        public override string Name { get; }

        public IMethod AddMethod(string methodName)
        {
            var m = new Method(this, methodName);
            ((IList<IMember>)Members).Add(m);
            return m;
        }
    }
}