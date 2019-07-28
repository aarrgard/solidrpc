using SolidRpc.OpenApi.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SolidRpc.OpenApi.Generator.Model.CSharp.Impl
{
    public class CSharpRepository : ICSharpRepository, ICSharpMember
    {

        public static (string, IList<string>, string) ReadType(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                return (fullName, null, null);
            }
            if (fullName.StartsWith(">"))
            {
                return (null, null, fullName.Substring(1));
            }
            var genIdxStart = fullName.IndexOf('<');
            if (genIdxStart == -1)
            {
                var genIdxEnd = fullName.IndexOf('>');
                if (genIdxEnd > -1)
                {
                    return (fullName.Substring(0, genIdxEnd), null, fullName.Substring(genIdxEnd));
                }
                else
                {
                    return (fullName, null, "");
                }
            }

            var genArgs = new List<string>();
            var genType = fullName.Substring(0, genIdxStart);
            var work = fullName.Substring(genIdxStart + 1);
            var rest = "";
            while (work != null)
            {
                string argType;
                IList<string> args;
                (argType, args, rest) = ReadType(work);
                if (!string.IsNullOrEmpty(argType))
                {
                    if (args == null)
                    {
                        genArgs.Add($"{argType}");
                    }
                    else
                    {
                        genArgs.Add($"{argType}<{string.Join(",", args)}>");
                    }
                }
                work = rest;
            }
            return (genType, genArgs, rest);
        }

        public CSharpRepository()
        {
            Namespaces = new ConcurrentDictionary<string, ICSharpMember>();
            ClassesAndInterfaces = new ConcurrentDictionary<string, ICSharpMember>();
        }
        public ConcurrentDictionary<string, ICSharpMember> Namespaces { get; }

        public ConcurrentDictionary<string, ICSharpMember> ClassesAndInterfaces { get; }

        public ICSharpMember Parent => null;

        public IEnumerable<ICSharpMember> Members => Namespaces.Values.Union(ClassesAndInterfaces.Values);

        public string Name => "";

        public string FullName => "";

        public ICSharpComment Comment => null;
        public void ParseComment(string comment) {  }

        public IEnumerable<ICSharpClass> Classes => ClassesAndInterfaces.Values.OfType<ICSharpClass>();

        public IEnumerable<ICSharpInterface> Interfaces => ClassesAndInterfaces.Values.OfType<ICSharpInterface>();

        public ICSharpNamespace GetNamespace(string fullName)
        {
            return (ICSharpNamespace)Namespaces.GetOrAdd(fullName, _ =>
            {
                var qn = new QualifiedName(_);
                var parent = (ICSharpMember)this;
                if(qn.Names.Count() > 1)
                {
                    parent = GetNamespace(qn.Namespace);
                }
                return new CSharpNamespace(parent, qn.Name ?? "");
            });
        }

        public ICSharpClass GetClass(string fullName)
        {
            var member = ClassesAndInterfaces.GetOrAdd(fullName, _ =>
            {
                var qn = new QualifiedName(_);
                var ns = GetNamespace(qn.Namespace);
                return new CSharpClass(ns, qn.Name, GetSystemType(_));
            });
            if(member is ICSharpClass clz)
            {
                return clz;
            }
            throw new Exception("Member is not a class:" + member.GetType().FullName);
        }

        public ICSharpInterface GetInterface(string fullName)
        {
            return (ICSharpInterface)ClassesAndInterfaces.GetOrAdd(fullName, _ =>
            {
                var qn = new QualifiedName(_);
                var ns = GetNamespace(qn.Namespace);
                return new CSharpInterface(ns, qn.Name, GetSystemType(_));
            });
        }

        public void AddMember(ICSharpMember member)
        {
            throw new NotImplementedException();
        }

        public ICSharpType GetType(string fullName)
        {
            if (ClassesAndInterfaces.TryGetValue(fullName, out ICSharpMember member))
            {
                return (ICSharpType)member;
            }
            var (genType, genArgs, rest) = ReadType(fullName);
            if (genArgs != null) genType = $"{genType}`{genArgs.Count}";
            var t = GetSystemType(genType);
            if (t == null)
            {
                return null;
            }
            else if (t.IsClass)
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
        private Type GetSystemType(string fullName)
        {
            var systemAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(o => !o.IsDynamic)
                .Where(o => o.IsFullyTrusted);
            foreach (Assembly a in systemAssemblies)
            {
                foreach (Type t in a.GetTypes())
                {
                    if(!t.FullName.StartsWith("System."))
                    {
                        continue;
                    }
                    if(t.FullName == fullName)
                    {
                        return t;
                    }
                }
            }
            switch (fullName)
            {
                case "void":
                    return typeof(void);
                case "bool":
                    return typeof(bool);
                case "short":
                    return typeof(short);
                case "int":
                    return typeof(int);
                case "long":
                    return typeof(long);
                case "float":
                    return typeof(float);
                case "double":
                    return typeof(double);
                case "string":
                    return typeof(string);
                default:
                    return null;
            }
        }

        public T GetParent<T>() where T : ICSharpMember
        {
            return default(T);
        }

        public void WriteCode(ICodeWriter codeWriter)
        {
            var prospects = Members.OfType<ICSharpType>();
            prospects = prospects.Where(o => o.RuntimeType == null);
            prospects = prospects.Where(o => o.EnumerableType == null);
            prospects = prospects.Where(o => o.TaskType == null);
            prospects = prospects.Where(o => !o.FullName.StartsWith("System."));
            prospects.ToList().ForEach(o =>
                {
                    o.WriteCode(codeWriter);
                });
        }

        public void GetNamespaces(ICollection<string> namespaces)
        {
            throw new NotImplementedException();
        }
    }
}
