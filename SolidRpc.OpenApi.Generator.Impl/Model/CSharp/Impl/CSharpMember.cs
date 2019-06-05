using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace SolidRpc.OpenApi.Generator.Model.CSharp.Impl
{
    public abstract class CSharpMember : ICSharpMember
    {
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
        public ICSharpMember Parent { get; }

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

        public ICSharpNamespace Namespace => GetParent<ICSharpNamespace>();

        protected IList<ICSharpMember> ProtectedMembers { get; }

        public IEnumerable<ICSharpMember> Members => ProtectedMembers;

        public string Name { get; }

        public string FullName { get; }

        public ICSharpComment Comment { get; protected set; }

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
            HandleTags("summary", comment, (p, c) => SetCommentSummary(c));
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

        protected virtual void SetCommentParameter(string parameterName, string comment)
        {
        }

        protected virtual void SetCommentSummary(string summary)
        {
            summary = summary.Trim();
            if(!string.IsNullOrEmpty(summary))
            {
                Comment = new CSharpComment(summary);
            }
        }

        public virtual void AddMember(ICSharpMember member)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ICSharpMethod> Methods => Members.OfType<ICSharpMethod>();

        public IEnumerable<ICSharpProperty> Properties => Members.OfType<ICSharpProperty>();

        public ICSharpType EnumerableType
        {
            get
            {
                var (genType, genArgs, rest) = CSharpRepository.ReadType(FullName);
                switch (genType)
                {
                    case "System.Collections.Generic.IEnumerable":
                        return GetParent<ICSharpRepository>().GetType(genArgs[0]);
                    default:
                        return null;
                }
                
            }
        }
        public ICSharpType TaskType
        {
            get
            {
                var (genType, genArgs, rest) = CSharpRepository.ReadType(FullName);
                switch (genType)
                {
                    case "System.Threading.Tasks.Task":
                        return GetParent<ICSharpRepository>().GetType(genArgs[0]);
                    default:
                        return null;
                }

            }
        }

        public virtual void GetNamespaces(ICollection<string> namespaces)
        {
            Members.ToList().ForEach(o => o.GetNamespaces(namespaces));
        }

        public abstract void WriteCode(ICodeWriter codeWriter);

        protected string SimplifyName(string fullName)
        {
            // find all usings in parents 
            var usings = new HashSet<string>();
            var work = Parent;
            while (work != null)
            {
                work.Members.OfType<ICSharpUsing>().ToList().ForEach(o => usings.Add(o.Name));
                work = work.Parent;
            }
            var prefix = usings.Where(o => fullName.StartsWith($"{o}."))
                .OrderByDescending(o => o.Length)
                .FirstOrDefault();
            if (prefix != null)
            {
                fullName = fullName.Substring(prefix.Length + 1);
            }
            //
            // handle generic types
            //
            var genStart = fullName.IndexOf('<');
            var genEnd = fullName.LastIndexOf('>');
            if (genStart > -1 && genEnd > -1)
            {
                return $"{fullName.Substring(0, genStart + 1)}{SimplifyName(fullName.Substring(genStart + 1, genEnd - genStart - 1))}{fullName.Substring(genEnd)}";
            }
            //
            // handle default(xxx)
            //
            if (fullName.StartsWith("default(") && fullName.EndsWith(")"))
            {
                return $"default({SimplifyName(fullName.Substring(8, fullName.Length - 9))})";
            }
            return fullName;
        }

    }
}