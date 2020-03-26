using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Implements the logic for the respository
    /// </summary>
    public class CSharpRepository : ICSharpRepository, ICSharpMember
    {
        private class TypeNameHandler
        {
            private StringBuilder _typeName = new StringBuilder();
            private StringBuilder _genArg = new StringBuilder();
            private IList<string> _genArgs;
            private int _genericTypeIdx = 0;
            internal void HandleChar(char c)
            {
                if (c == '<')
                {
                    _genericTypeIdx++;
                    if(_genericTypeIdx == 1)
                    {
                        return;
                    }
                }
                if (c == '>')
                {
                    _genericTypeIdx--;
                    if (_genericTypeIdx == 0)
                    {
                        PushGenArg();
                        return;
                    }
                }
                if (c == ',')
                {
                    PushGenArg();
                    return;
                }
                if (_genericTypeIdx == 0)
                {
                    _typeName.Append(c);
                }
                else
                {
                    _genArg.Append(c);
                }
            }

            private void PushGenArg()
            {
                if (_genArgs == null) _genArgs = new List<string>();
                _genArgs.Add(_genArg.ToString().Trim());
                _genArg.Clear();
            }

            internal (string, IList<string>) GetResult()
            {
                if (_genericTypeIdx != 0) throw new Exception("type index not at 0");
                return (_typeName.ToString().Trim(), _genArgs);
            }
        }

        private ConcurrentDictionary<string, Type> s_systemTypes = new ConcurrentDictionary<string, Type>();
        
        /// <summary>
        /// Parses supplied name into a generic representation.
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static (string, IList<string>) ReadType(string fullName)
        {
            var tnh = new TypeNameHandler();
            for (int i = 0; i < fullName.Length; i++)
            {
                tnh.HandleChar(fullName[i]);
            }
            return tnh.GetResult();
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public CSharpRepository()
        {
            Namespaces = new ConcurrentDictionary<string, ICSharpNamespace>();
            Types = new ConcurrentDictionary<string, ICSharpMember>();
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
            GetClass(typeof(decimal), "decimal");
            GetClass(typeof(string), "string");
            GetClass(typeof(DateTime), typeof(DateTime).FullName);
            GetClass(typeof(DateTimeOffset), typeof(DateTimeOffset).FullName);
            LoadSystemTypes(typeof(int).Assembly);
            LoadSystemTypes(typeof(Uri).Assembly);
            LoadSystemTypes(typeof(Guid).Assembly);
            LoadSystemTypes(typeof(StringValues).Assembly);
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
        public ConcurrentDictionary<string, ICSharpMember> Types { get; }

        /// <summary>
        /// The parent
        /// </summary>
        public ICSharpMember Parent => null;

        /// <summary>
        /// All the members(classes, interfaces and namespaces)
        /// </summary>
        public IEnumerable<ICSharpMember> Members => Namespaces.Values.Union(Types.Values);

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
        public IEnumerable<ICSharpClass> Classes => Types.Values.OfType<ICSharpClass>();

        /// <summary>
        /// Returns the interfaces
        /// </summary>
        public IEnumerable<ICSharpInterface> Interfaces => Types.Values.OfType<ICSharpInterface>();

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
            var member = Types.GetOrAdd(fullName, _ =>
            {
                // create runtime type
                Type runtimeType = null;
                var (typeName, genArgs) = ReadType(_);
                if(genArgs != null)
                {
                    var genType = GetType($"{typeName}`{genArgs.Count}")?.RuntimeType;
                    var genArgTypes = genArgs.Select(o => GetType(o)?.RuntimeType).ToArray();
                    if(genType != null && genArgTypes.All(o => o!=null))
                    {
                        runtimeType = genType.MakeGenericType(genArgTypes);
                    }
                }

                var qn = new QualifiedName(_);
                var ns = GetNamespace(qn.Namespace);
                return new CSharpClass(ns, qn.Name, runtimeType);
            });
            if (member is ICSharpClass clz)
            {
                return clz;
            }
            throw new Exception($"Member({fullName}) is not a class:{member.GetType().FullName}");
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
            return (ICSharpClass)Types.GetOrAdd(typeName, _ =>
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
            return (ICSharpInterface)Types.GetOrAdd(fullName, _ =>
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
            return (ICSharpInterface)Types.GetOrAdd(type.FullName, _ =>
            {
                var qn = new QualifiedName(_);
                var ns = GetNamespace(qn.Namespace);
                return new CSharpInterface(ns, qn.Name, type);
            });
        }

        /// <summary>
        /// Returns the enum type
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public ICSharpEnum GetEnum(string fullName)
        {
            return (ICSharpEnum)Types.GetOrAdd(fullName, _ =>
            {
                var qn = new QualifiedName(_);
                var ns = GetNamespace(qn.Namespace);
                return new CSharpEnum(ns, qn.Name, null);
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
            if (Types.TryGetValue(fullName, out member))
            {
                return (ICSharpType)member;
            }
            var (genType, genArgs) = ReadType(fullName);
            if (genArgs != null) genType = $"{genType}`{genArgs.Count}";
            if (Types.TryGetValue(genType, out member))
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
