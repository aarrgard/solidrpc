using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class CodeGenerator : ICodeGenerator
    {
        public CodeGenerator()
        {
            DefaultNamespace = new Namespace(this, "");
        }

        public INamespace DefaultNamespace { get; }

        public string Name => "";

        public IList<IMember> Members => DefaultNamespace.Members;

        public IMember Parent => null;

        public string FullName => "";

        public string Namespace => "";

        public IClass CreateGenericType(string genericTypeDef, string classFullName)
        {
            var genQName = new QualifiedName(genericTypeDef);
            var genNs = GetNamespace(genQName.Namespace);
            return genNs.GetClass($"{genQName.Name}<{classFullName}>");
        }

        public INamespace GetNamespace(string ns)
        {
            var names = new QualifiedName(ns).Names;
            var @namespace = DefaultNamespace;
            foreach (var name in names)
            {
                @namespace = @namespace.GetNamespace(name);
            }
            return @namespace;
        }

        public void GetNamespaces(ICollection<string> namespaces)
        {
            DefaultNamespace.GetNamespaces(namespaces);
        }

        public T GetParent<T>() where T : IMember
        {
            throw new System.NotImplementedException();
        }

        public void WriteCode(ICodeWriter codeWriter)
        {
            DefaultNamespace.WriteCode(codeWriter);
        }
    }
}
