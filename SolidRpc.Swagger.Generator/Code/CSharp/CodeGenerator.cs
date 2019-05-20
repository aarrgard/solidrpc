using System;
using System.Collections.Concurrent;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class CodeGenerator : ICodeGenerator
    {
        public CodeGenerator()
        {
            Namespaces = new ConcurrentDictionary<string, INamespace>();
        }

        public ConcurrentDictionary<string, INamespace> Namespaces { get; }

        public INamespace GetNamespace(string ns)
        {
            return Namespaces.GetOrAdd(ns, (_) => new Namespace());
        }

        public void WriteCode(ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }
    }
}
