using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public abstract class Member : IMember
    {
        public Member(IMember parent)
        {
            Parent = parent;
            Members = new List<IMember>();
        }
        public IMember Parent { get; }

        public IList<IMember> Members { get; }

        public abstract string Name { get; }

        public T GetParent<T>() where T : IMember
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

        public abstract void WriteCode(ICodeWriter codeWriter);

        protected string SimplifyName(string fullName)
        {
            // find all usings in parents 
            var usings = new HashSet<string>();
            var work = Parent;
            while(work != null)
            {
                work.Members.OfType<IUsing>().ToList().ForEach(o => usings.Add(o.Name));
                work = work.Parent;
            }
            var prefix = usings.Where(o => fullName.StartsWith($"{o}."))
                .OrderByDescending(o => o.Length)
                .FirstOrDefault();
            if(prefix != null)
            {
                fullName = fullName.Substring(prefix.Length+1);
            }
            //
            // handle generic types
            //
            var genStart = fullName.IndexOf('<');
            var genEnd = fullName.LastIndexOf('>');
            if (genStart > -1 && genEnd > -1)
            {
                return $"{fullName.Substring(0, genStart+1)}{SimplifyName(fullName.Substring(genStart+1, genEnd-genStart-1))}{fullName.Substring(genEnd)}";
            }
            //
            // handle default(xxx)
            //
            if(fullName.StartsWith("default(") && fullName.EndsWith(")"))
            {
                return $"default({SimplifyName(fullName.Substring(8, fullName.Length-9))})";
            }
            return fullName;
        }

    }
}
