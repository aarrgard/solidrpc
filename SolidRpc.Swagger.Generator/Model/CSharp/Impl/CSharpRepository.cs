using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
{
    public class CSharpRepository : ICSharpRepository, ICSharpMember
    {
        public CSharpRepository()
        {
            Members = new ConcurrentDictionary<string, ICSharpMember>();
        }
        public ConcurrentDictionary<string, ICSharpMember> Members { get; }

        public ICSharpMember Parent => null;

        IEnumerable<ICSharpMember> ICSharpMember.Members => Members.Values;

        public string Name => "";

        public string FullName => "";

        public string Comment { get; set; }

        public ICSharpNamespace GetNamespace(string fullName)
        {
            return (ICSharpNamespace)Members.GetOrAdd(fullName, _ =>
            {
                var qn = new QualifiedName(_);
                var parent = (ICSharpMember)this;
                if(qn.Names.Count() > 1)
                {
                    parent = GetNamespace(qn.Namespace);
                }
                return new CSNamespace(parent, qn.Name);
            });
        }

        public ICSharpClass GetClass(string fullName)
        {
            return (ICSharpClass) Members.GetOrAdd(fullName, _ =>
            {
                var qn = new QualifiedName(_);
                var ns = GetNamespace(qn.Namespace);
                return new CSharpClass(ns, qn.Name);
            });
        }

        public ICSharpInterface GetInterface(string fullName)
        {
            return (ICSharpInterface)Members.GetOrAdd(fullName, _ =>
            {
                var qn = new QualifiedName(_);
                var ns = GetNamespace(qn.Namespace);
                return new CSharpInterface(ns, qn.Name);
            });
        }

        public void AddMember(ICSharpMember member)
        {
            throw new NotImplementedException();
        }

        public ICSharpType GetType(string fullName)
        {
            ICSharpMember member;
            if(Members.TryGetValue(fullName, out member))
            {
                return (ICSharpType) member;
            }
            return GetSystemType(fullName);
        }

        private ICSharpType GetSystemType(string fullName)
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in a.GetTypes())
                {
                    if(t.FullName == fullName)
                    {
                        if (t.IsClass)
                        {
                            return GetClass(fullName);
                        }
                        else if (t.IsInterface)
                        {
                            return GetInterface(fullName);
                        }
                        else if (t.IsValueType)
                        {
                            return GetClass(fullName);
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
            }
            switch (fullName)
            {
                case "bool":
                case "short":
                case "int":
                case "long":
                case "string":
                    return GetClass("string");
                default:
                    return null;
            }
        }
    }
}
