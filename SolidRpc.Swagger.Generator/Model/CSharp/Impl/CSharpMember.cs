using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
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

        protected IList<ICSharpMember> ProtectedMembers { get; }

        public IEnumerable<ICSharpMember> Members => ProtectedMembers;

        public string Name { get; }

        public string FullName { get; }

        public string Comment { get; set; }

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
    }
}