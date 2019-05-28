using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
