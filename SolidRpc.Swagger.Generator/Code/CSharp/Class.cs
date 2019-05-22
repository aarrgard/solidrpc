using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class Class : ClassOrInterface, IClass
    {
        public Class(Namespace ns, string name) : base(ns)
        {
            Namespace = ns;
            Name = name;
        }
        public override INamespace Namespace { get; }

        public override string Name { get; }

        public IEnumerable<IProperty> Properties => Members.OfType<IProperty>();

        public IProperty AddProperty(string propertyName, IClass propType)
        {
            var property = new Property(this, propType, propertyName);
            Members.Add(property);
            return property;
        }
    }
}