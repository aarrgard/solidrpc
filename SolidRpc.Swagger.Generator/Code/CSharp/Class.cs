using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class Class : ClassOrInterface, IClass
    {
        public Class(Namespace ns, string name)
        {
            Namespace = ns;
            Name = name;
        }
        public override INamespace Namespace { get; }

        public override string Name { get; }

        public override IEnumerable<IMember> Members => new IMember[0];
    }
}