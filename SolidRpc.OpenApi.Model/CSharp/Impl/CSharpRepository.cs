using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Implements the logic for the respository
    /// </summary>
    public class CSharpRepository : ICSharpRepository, ICSharpMember
    {
        private ConcurrentDictionary<string, Type> s_systemTypes = new ConcurrentDictionary<string, Type>();
        
        /// <summary>
        /// Parses supplied name into a generic representation.
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public CSharpRepository()
        {
            Namespaces = new ConcurrentDictionary<string, ICSharpNamespace>();
            ClassesAndInterfaces = new ConcurrentDictionary<string, ICSharpMember>();
            LoadSystemTypes();
        }

        private void LoadSystemTypes()
        {
            GetClass(typeof(void), "void");
            GetClass(typeof(bool), "bool");
            GetClass(typeof(short), "short");
            GetClass(typeof(int), "int");
            GetClass(typeof(long), "long");
            GetClass(typeof(float), "float");
            GetClass(typeof(double), "double");
            GetClass(typeof(string), "string");
            LoadSystemTypes(typeof(int).Assembly);
            LoadSystemTypes(typeof(Uri).Assembly);
        }

        private void LoadSystemTypes(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsNotPublic)
                {
                    continue;
                }
                if (type.IsNested)
                {
                    continue;
                }
                ICSharpType added;
                if (type.IsInterface)
                {
                    added = GetInterface(type);
                }
                else if (type.IsClass)
                {
                    added = GetClass(type);
                }
                else if (type.IsValueType)
                {
                    added = GetClass(type);
                }
                else
                {
                    continue;
                }
                if (added.RuntimeType == null)
                {
                    throw new Exception($"Failed to fetch runtime type for system type {added.FullName}");
                }
            }
        }

        /// <summary>
        /// All the namespaces
        /// </summary>
        public ConcurrentDictionary<string, ICSharpNamespace> Namespaces { get; }


        /// <summary>
        /// All the classes and interfaces 
        /// </summary>
        public ConcurrentDictionary<string, ICSharpMember> ClassesAndInterfaces { get; }

        /// <summary>
        /// The parent
        /// </summary>
        public ICSharpMember Parent => null;

        /// <summary>
        /// All the members(classes, interfaces and namespaces)
        /// </summary>
        public IEnumerable<ICSharpMember> Members => Namespaces.Values.Union(ClassesAndInterfaces.Values);

        /// <summary>
        /// The name(empty)
        /// </summary>
        public string Name => "";

        /// <summary>
        /// The full name(empty)
        /// </summary>
        public string FullName => "";

        /// <summary>
        /// The comment(empty)
        /// </summary>
        public ICSharpComment Comment => null;

        /// <summary>
        /// Parses supplied comment
        /// </summary>
        /// <param name="comment"></param>
        public void ParseComment(string comment) {  }

        /// <summary>
        /// Returns the classes
        /// </summary>
        public IEnumerable<ICSharpClass> Classes => ClassesAndInterfaces.Values.OfType<ICSharpClass>();

        /// <summary>
        /// Returns the interfaces
        /// </summary>
        public IEnumerable<ICSharpInterface> Interfaces => ClassesAndInterfaces.Values.OfType<ICSharpInterface>();

        IEnumerable<ICSharpNamespace> ICSharpRepository.Namespaces => Namespaces.Values.OfType<ICSharpNamespace>();

        /// <summary>
        /// REturns the namespace if it exists
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        public bool TryGetNamespace(string fullName, out ICSharpNamespace ns)
        {
            return Namespaces.TryGetValue(fullName, out ns);
        }


        /// <summary>
        /// Returns the namespace for supplied qualified name
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Returns the class for supplied qualified name
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public ICSharpClass GetClass(string fullName)
        {
            var member = ClassesAndInterfaces.GetOrAdd(fullName, _ =>
            {
                var qn = new QualifiedName(_);
                var ns = GetNamespace(qn.Namespace);
                return new CSharpClass(ns, qn.Name, null);
            });
            if (member is ICSharpClass clz)
            {
                return clz;
            }
            throw new Exception("Member is not a class:" + member.GetType().FullName);
        }

        /// <summary>
        /// Returns the class for supplied qualified name
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public ICSharpClass GetClass(Type type, string typeName = null)
        {
            if (typeName == null) typeName = type.FullName;
            return (ICSharpClass)ClassesAndInterfaces.GetOrAdd(typeName, _ =>
            {
                var qn = new QualifiedName(_);
                var ns = GetNamespace(qn.Namespace);
                return new CSharpClass(ns, qn.Name, type);
            });
        }


        /// <summary>
        /// Returns the interface for supplied qualified name
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public ICSharpInterface GetInterface(string fullName)
        {
            return (ICSharpInterface)ClassesAndInterfaces.GetOrAdd(fullName, _ =>
            {
                var qn = new QualifiedName(_);
                var ns = GetNamespace(qn.Namespace);
                return new CSharpInterface(ns, qn.Name, null);
            });
        }

        /// <summary>
        /// Returns the interface for supplied type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ICSharpInterface GetInterface(Type type)
        {
            return (ICSharpInterface)ClassesAndInterfaces.GetOrAdd(type.FullName, _ =>
            {
                var qn = new QualifiedName(_);
                var ns = GetNamespace(qn.Namespace);
                return new CSharpInterface(ns, qn.Name, type);
            });
        }

        /// <summary>
        /// Adds a member(not implemented)
        /// </summary>
        /// <param name="member"></param>
        public void AddMember(ICSharpMember member)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns the type for supplied qualified name
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public ICSharpType GetType(string fullName)
        {
            ICSharpMember member;
            if (ClassesAndInterfaces.TryGetValue(fullName, out member))
            {
                return (ICSharpType)member;
            }
            var (genType, genArgs, rest) = ReadType(fullName);
            if (genArgs != null) genType = $"{genType}`{genArgs.Count}";
            if (ClassesAndInterfaces.TryGetValue(genType, out member))
            {
                if(member is ICSharpInterface)
                {
                    return GetInterface(fullName);
                }
                else
                {
                    return GetClass(fullName);
                }
            }
            return null;
            //var t = GetSystemType(genType);
            //if (t == null)
            //{
            //    return null;
            //}
            //else if (t.IsClass)
            //{
            //    return GetClass(fullName);
            //}
            //else if (t.IsInterface)
            //{
            //    return GetInterface(fullName);
            //}
            //else if (t.IsValueType)
            //{
            //    return GetClass(fullName);
            //}
            //else
            //{
            //    throw new Exception();
            //}

        }

        /// <summary>
        /// Returns the parent of supplied type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetParent<T>() where T : ICSharpMember
        {
            return default(T);
        }

        /// <summary>
        /// Emits the code in the repository to supplied writer.
        /// </summary>
        /// <param name="codeWriter"></param>
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

        /// <summary>
        /// populates supplied collection with all the namespaces.
        /// </summary>
        /// <param name="namespaces"></param>
        public void GetNamespaces(IDictionary<string, HashSet<string>> namespaces)
        {
            throw new NotImplementedException();
        }
    }
}
