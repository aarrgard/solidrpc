using SolidRpc.Swagger.Generator.Code.Binder;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class Namespace : INamespace, IQualifiedMember
    {
        public Namespace(IQualifiedMember parent, string name)
        {
            Name = name;
            FullName = new QualifiedName(parent?.FullName, name).ToString();
            Members = new ConcurrentDictionary<string, IQualifiedMember>();
        }

        private ConcurrentDictionary<string, IQualifiedMember> Members { get; }

        IEnumerable<IMember> IMember.Members => Members.Values;

        public string Name { get; }

        public string FullName { get; }

        public IClass GetClass(string className)
        {
            return (IClass)Members.GetOrAdd(className, _ => new Class(this, _));
        }

        public IInterface GetInterface(string interfaceName)
        {
            return (IInterface)Members.GetOrAdd(interfaceName, _ => new Interface(this, _));
        }

        public INamespace GetNamespace(string interfaceName)
        {
            return (INamespace)Members.GetOrAdd(interfaceName, _ => new Namespace(this, _));
        }

        public void WriteCode(ICodeWriter codeWriter)
        {
            Members.Values
                .Where(o => !SwaggerDefinition.ReservedNames.Contains(o.FullName))
                .Where(o => !o.Name.Contains('<')) // do not generate generic types
                .ToList().ForEach(o => o.WriteCode(codeWriter));
        }
    }
}