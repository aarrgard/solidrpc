using SolidRpc.Swagger.Generator.Code.Binder;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class Namespace : Member, INamespace, IQualifiedMember
    {
        public Namespace(IQualifiedMember parent, string name) : base(parent)
        {
            if (parent.FullName.Contains("<"))
            {
                throw new System.Exception("Supplied namespace contains illegal characters in name:" + parent.FullName);
            }
            if (name.Contains("<"))
            {
                throw new System.Exception("Supplied name contains illegal characters:" + name);
            }
            Name = name;
            FullName = new QualifiedName(parent?.FullName, name).QName;
            NamespaceDummy = new QualifiedName(parent?.FullName, name).Namespace;
        }

        public override string Name { get; }

        public string FullName { get; }

        private string NamespaceDummy { get; }
        string IQualifiedMember.Namespace => NamespaceDummy;

        public IClass GetClass(string className)
        {
            var cls = Members.OfType<IClass>().Where(o => o.Name == className).SingleOrDefault();
            if(cls == null)
            {
                Members.Add(cls = new Class(this, className));
            }
            return cls;
        }

        public IInterface GetInterface(string interfaceName)
        {
            var ifz = Members.OfType<IInterface>().Where(o => o.Name == interfaceName).SingleOrDefault();
            if (ifz == null)
            {
                Members.Add(ifz = new Interface(this, interfaceName));
            }
            return ifz;
        }

        public INamespace GetNamespace(string namespaceName)
        {
            var ns = Members.OfType<INamespace>().Where(o => o.Name == namespaceName).SingleOrDefault();
            if (ns == null)
            {
                Members.Add(ns = new Namespace(this, namespaceName));
            }
            return ns;
        }

        public override void WriteCode(ICodeWriter codeWriter)
        {
            Members.OfType<IQualifiedMember>()
                .Where(o => !SwaggerDefinition.ReservedNames.Contains(o.FullName))
                .Where(o => !o.Name.Contains('<')) // do not generate generic types
                .ToList().ForEach(o => o.WriteCode(codeWriter));
        }
    }
}