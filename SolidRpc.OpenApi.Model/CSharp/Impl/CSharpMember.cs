﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Base class for the members
    /// </summary>
    public abstract class CSharpMember : ICSharpMember
    {

        /// <summary>
        /// Adds the suplied namespace and name to the namespaces
        /// </summary>
        /// <param name="namespaces"></param>
        /// <param name="fullName"></param>
        /// <param name="name"></param>
        public static void AddNamespacesFromName(IDictionary<string, HashSet<string>> namespaces, string fullName, string name)
        {
            if (!namespaces.TryGetValue(fullName, out HashSet<string> namesInNamespace))
            {
                namespaces[fullName] = namesInNamespace = new HashSet<string>();
            }
            namesInNamespace.Add(name);
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        public CSharpMember(ICSharpMember parent, string name)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ProtectedMembers = new List<ICSharpMember>();
            var parentFullName = Parent.FullName;
            if (!string.IsNullOrEmpty(parentFullName))
            {
                FullName = $"{parentFullName}.{Name}";
            }
            else
            {
                FullName = Name;
            }
        }

        /// <summary>
        /// The parent
        /// </summary>
        public ICSharpMember Parent { get; }

        /// <summary>
        /// Returns the first parent of specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetParent<T>() where T : ICSharpMember
        {
            if(Parent is T t)
            {
                return t;
            }
            if(Parent is null)
            {
                return default(T);
            }
            return Parent.GetParent<T>();
        }

        /// <summary>
        /// Returns the namespace that this member belongs to
        /// </summary>
        public ICSharpNamespace Namespace => GetParent<ICSharpNamespace>();

        /// <summary>
        /// Returns the protected members
        /// </summary>
        protected IList<ICSharpMember> ProtectedMembers { get; }

        /// <summary>
        /// Returns all the members
        /// </summary>
        public IEnumerable<ICSharpMember> Members => ProtectedMembers;

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The full name
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// The comment
        /// </summary>
        public ICSharpComment Comment { get; protected set; }

        /// <summary>
        /// Parses supplied comment
        /// </summary>
        /// <param name="comment"></param>
        public void ParseComment(string comment)
        {
            var sb = new StringBuilder();
            using (var sr = new StringReader(comment))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if(line.StartsWith("///"))
                    {
                        line = line.Substring(3).Trim();
                    }
                    sb.AppendLine(line);
                }
            }
            comment = sb.ToString();
            string summary = "";
            ICSharpCommentExternalDoc externalDoc = null;
            var exceptions = new List<ICSharpCommentException>(); ;
            HandleTags("summary", comment, (p, c) => summary = c.Trim());
            HandleTags("a", comment, (p, c) => {
                if (p.ContainsKey("href"))
                {
                    if (!string.IsNullOrEmpty(p["href"]) || !string.IsNullOrEmpty(c))
                    {
                        externalDoc = new CSharpCommentExternalDoc(p["href"], c);
                    }
                }
            });
            HandleTags("exception", comment, (p, c) => {
                if (p.ContainsKey("cref"))
                {
                    if (!string.IsNullOrEmpty(p["cref"]) || !string.IsNullOrEmpty(c))
                    {
                        exceptions.Add(new CSharpCommentException(p["cref"], c));
                    }
                }
            });

            Comment = new CSharpComment(summary, exceptions, externalDoc);

            HandleTags("param", comment, (p, c) => {
                if(p.ContainsKey("name"))
                {
                    SetCommentParameter(p["name"], c);
                }
            });
        }

        private void HandleTags(string tagName, string comment, Action<IDictionary<string, string>, string> action)
        {
            var start = 0;
            while(true)
            {
                var startOfTag = comment.IndexOf($"<{tagName}", start);
                if(startOfTag == -1)
                {
                    return;
                }
                var endOfStartTag = comment.IndexOf($">", startOfTag);
                if (endOfStartTag == -1)
                {
                    return;
                }
                var parameters = GetParameters(comment.Substring(startOfTag + $"<{tagName}".Length, endOfStartTag - startOfTag - $"<{tagName}".Length));
                var endOfTag = comment.IndexOf($"</{tagName}>", endOfStartTag+1);
                if(endOfTag == -1)
                {
                    return;
                }
                var tagContent = comment.Substring(endOfStartTag + 1, endOfTag - endOfStartTag - 1);
                action(parameters, tagContent);
                start = endOfTag;
            }
        }

        private IDictionary<string, string> GetParameters(string tagContent)
        {
            var dict = new Dictionary<string, string>();
            PopulateParameters(dict, tagContent);
            return dict;
        }

        private void PopulateParameters(Dictionary<string, string> dict, string tagContent)
        {
            var eqIdx = tagContent.IndexOf('=');
            if(eqIdx == -1)
            {
                return;
            }
            var q1 = tagContent.IndexOf('\"', eqIdx);
            if (q1 == -1)
            {
                return;
            }
            var q2 = tagContent.IndexOf('\"', q1+1);
            if (q2 == -1)
            {
                return;
            }
            dict[tagContent.Substring(0, eqIdx).Trim()] = tagContent.Substring(q1+1,q2-q1-1);
            PopulateParameters(dict, tagContent.Substring(q2+1));
        }

        /// <summary>
        /// Sets a comment parameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="comment"></param>
        protected virtual void SetCommentParameter(string parameterName, string comment)
        {
        }

        /// <summary>
        /// Adds a member
        /// </summary>
        /// <param name="member"></param>
        public virtual void AddMember(ICSharpMember member)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Returns the methods
        /// </summary>
        public IEnumerable<ICSharpMethod> Methods => Members.OfType<ICSharpMethod>();

        /// <summary>
        /// Returns the properties
        /// </summary>
        public IEnumerable<ICSharpProperty> Properties => Members.OfType<ICSharpProperty>();

        /// <summary>
        /// Returns the member of the enumerable type(if this is an enumerable type)
        /// </summary>
        public ICSharpType EnumerableType
        {
            get
            {
                var fullName = FullName;
                if(fullName.EndsWith("[]"))
                {
                    return GetParent<ICSharpRepository>().GetType(fullName.Substring(0,fullName.Length-2)) ?? throw new Exception("Array type does not exist in repository.");
                }
                var (genType, genArgs) = CSharpRepository.ReadType(fullName);
                switch (genType)
                {
                    case "System.Collections.Generic.IEnumerable":
                        return GetParent<ICSharpRepository>().GetType(genArgs[0]) ?? throw new Exception("Generic argument does not exist in repository.");
                    default:
                        return null;
                }
                
            }
        }

        /// <summary>
        /// Returns the type as a task type(if this is a task type).
        /// </summary>
        public ICSharpType TaskType
        {
            get
            {
                var (genType, genArgs) = CSharpRepository.ReadType(FullName);
                switch (genType)
                {
                    case "System.Threading.Tasks.Task":
                        return GetParent<ICSharpRepository>().GetType(genArgs[0]);
                    default:
                        return null;
                }

            }
        }

        /// <summary>
        /// Returns the nullable type(if this is a nullable type)
        /// </summary>
        public ICSharpType NullableType
        {
            get
            {
                var (genType, genArgs) = CSharpRepository.ReadType(FullName);
                switch (genType)
                {
                    case "System.Nullable":
                        return GetParent<ICSharpRepository>().GetType(genArgs[0]);
                    default:
                        return null;
                }

            }
        }

        /// <summary>
        /// populates the namespace dictionary
        /// </summary>
        /// <param name="namespaces"></param>
        public virtual void GetNamespaces(IDictionary<string, HashSet<string>> namespaces)
        {
            Members.ToList().ForEach(o => o.GetNamespaces(namespaces));
        }

        /// <summary>
        /// Adds the namespaces of supplied type. This method also
        /// adds the namespaces of the generic type arguments to the
        /// supplied collection.
        /// </summary>
        /// <param name="namespaces"></param>
        /// <param name="csType"></param>
        protected void AddNamespacesFromName(IDictionary<string, HashSet<string>> namespaces, ICSharpType csType)
        {
            AddNamespacesFromName(namespaces, csType.Namespace.FullName, csType.Name);

            var genArgs = csType.GetGenericArguments();
            if(genArgs != null)
            {
                foreach (var genType in genArgs)
                {
                    AddNamespacesFromName(namespaces, genType);
                }
            }
        }

        /// <summary>
        /// Emits the code to supplied writer
        /// </summary>
        /// <param name="codeWriter"></param>
        public abstract void WriteCode(ICodeWriter codeWriter);

        /// <summary>
        /// Simplifies the supplied name based on the current using
        /// directives and namespace.
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        protected string SimplifyName(string fullName)
        {
            //
            // handle default(xxx)
            //
            if (fullName.StartsWith("default(") && fullName.EndsWith(")"))
            {
                return $"default({SimplifyName(fullName.Substring(8, fullName.Length - 9))})";
            }

            // find all usings in parents 
            var usings = new HashSet<string>();
            var work = Parent;
            while (work != null)
            {
                work.Members.OfType<ICSharpUsing>().ToList().ForEach(o => usings.Add(o.Name));
                work = work.Parent;
            }

            // parse name
            var (typeName, genArgs) = CSharpRepository.ReadType(fullName);

            if(typeName == "System.Nullable" && genArgs.Count() == 1)
            {
                return $"{SimplifyName(genArgs.First())}?";
            }

            //
            // find prefix in "using" declarations that the typename
            // starts with.
            //
            var prefix = usings.Where(o => typeName.StartsWith($"{o}."))
                .OrderByDescending(o => o.Length)
                .FirstOrDefault();
            if (prefix != null)
            {
                var simplifiedName = typeName.Substring(prefix.Length + 1);

                // make sure that we do not have another match on this name in another using directive
                var cSharpRepo = GetParent<ICSharpRepository>();
                var resolvedTypes = usings
                    .Select(o => cSharpRepo.GetType($"{o}.{simplifiedName}"))
                    .Where(o => o != null);
                if(resolvedTypes.Count() < 2)
                {
                    if (genArgs != null)
                    {
                        simplifiedName = $"{simplifiedName}<{string.Join(",", genArgs.Select(o => SimplifyName(o)))}>";
                    }
                    simplifiedName = $"{simplifiedName}";
                    return simplifiedName;
                }
            }

            return fullName;
        }


        /// <summary>
        /// Writes the summary
        /// </summary>
        /// <param name="codeWriter"></param>
        protected void WriteSummary(ICodeWriter codeWriter)
        {
            var comment = Comment?.Summary ?? "";
            codeWriter.Indent("/// ");
            codeWriter.Emit($"<summary>{codeWriter.NewLine}");
            codeWriter.Emit($"{HttpUtility.HtmlEncode(comment)}{codeWriter.NewLine}");
            codeWriter.Emit($"</summary>{codeWriter.NewLine}");
            if (Comment?.ExternalDoc != null)
            {
                codeWriter.Emit($"<a href=\"{Comment.ExternalDoc.Url}\">{Comment.ExternalDoc.Description}</a>{codeWriter.NewLine}");
            }
            codeWriter.Unindent();
        }

        /// <summary>
        /// Writes the attributes
        /// </summary>
        /// <param name="codeWriter"></param>
        protected void WriteAttributes(ICodeWriter codeWriter)
        {
            Members.OfType<ICSharpAttribute>().ToList().ForEach(o =>
            {
                o.WriteCode(codeWriter);
            });
        }

    }
}