using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class CodeGenerator : ICodeGenerator
    {
        public CodeGenerator()
        {
            DefaultNamespace = new Namespace(null, "");
        }

        public INamespace DefaultNamespace { get; }

        public string Name => "";

        public IEnumerable<IMember> Members => DefaultNamespace.Members;

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

        public void WriteCode(ICodeWriter codeWriter)
        {
            DefaultNamespace.WriteCode(codeWriter);
        }
    }
}
