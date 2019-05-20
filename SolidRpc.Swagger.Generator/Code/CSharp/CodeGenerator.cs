using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class CodeGenerator : ICodeGenerator
    {
        public CodeGenerator()
        {
            Namespaces = new ConcurrentDictionary<string, INamespace>();
        }

        public ConcurrentDictionary<string, INamespace> Namespaces { get; }

        public string Name => "";

        public IEnumerable<IMember> Members => Namespaces.Values;

        public INamespace GetNamespace(string ns)
        {
            return Namespaces.GetOrAdd(ns, (_) => new Namespace(ns));
        }

        public void WriteCode(ICodeWriter codeWriter)
        {
            Members.ToList().ForEach(o => o.WriteCode(codeWriter));
        }
    }
}
