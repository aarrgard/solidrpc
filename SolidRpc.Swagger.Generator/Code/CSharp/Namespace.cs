using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class Namespace : INamespace
    {
        public Namespace(string name)
        {
            Members = new ConcurrentDictionary<string, IMember>();
        }

        private ConcurrentDictionary<string, IMember> Members { get; }

        IEnumerable<IMember> IMember.Members => Members.Values;

        public string Name { get; }

        public IClass GetClass(string className)
        {
            return (IClass)Members.GetOrAdd(className, _ => new Class(this, _));
        }

        public IInterface GetInterface(string interfaceName)
        {
            return (IInterface)Members.GetOrAdd(interfaceName, _ => new Interface(this, _));
        }

        public void WriteCode(ICodeWriter codeWriter)
        {
            Members.Values
                .Where(o => !IsReservedName(o.Name))
                .ToList().ForEach(o => o.WriteCode(codeWriter));
        }

        private bool IsReservedName(string name)
        {
            switch(name)
            {
                case "void":
                    return true;
                default:
                    return false;
            }
        }
    }
}