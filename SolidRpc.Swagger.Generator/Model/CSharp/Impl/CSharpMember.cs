using System.Collections.Generic;

namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
{
    public abstract class CSharpMember : ICSharpMember
    {
        public CSharpMember(ICSharpMember parent, string name)
        {
            Parent = parent;
            Name = name;
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

        protected IList<ICSharpMember> ProtectedMembers { get; }

        public IEnumerable<ICSharpMember> Members => ProtectedMembers;

        public string Name { get; }

        public string FullName { get; }

        public string Comment { get; set; }

        public virtual void AddMember(ICSharpMember member)
        {
            throw new System.NotImplementedException();
        }
    }
}