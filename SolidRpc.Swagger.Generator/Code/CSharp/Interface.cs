using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class Interface : ClassOrInterface, IInterface
    {
        public Interface(Namespace ns, string name)
        {
            Namespace = ns;
            Name = name;
            Members = new List<IMember>();
        }
        public override string Name { get; }

        public override INamespace Namespace { get; }

        public override IEnumerable<IMember> Members { get; }

        public IMethod AddMethod(string methodName)
        {
            var m = new Method(this, methodName);
            ((IList<IMember>)Members).Add(m);
            return m;
        }
    }
}