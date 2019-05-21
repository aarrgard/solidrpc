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
            Members = new List<IMember>();
        }
        public override INamespace Namespace { get; }

        public override string Name { get; }

        public override IEnumerable<IMember> Members { get; }

        public IEnumerable<IProperty> Properties => Members.OfType<IProperty>();

        public void AddProperty(string propertyName, IClass propType)
        {
            ((IList<IMember>)Members).Add(new Property(propType, propertyName));
        }
    }
}