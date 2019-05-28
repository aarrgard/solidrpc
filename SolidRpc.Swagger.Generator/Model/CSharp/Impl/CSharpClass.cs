namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
{
    public class CSharpClass : CSharpMember, ICSharpClass
    {
        public CSharpClass(ICSharpNamespace ns, string name) : base(ns, name)
        {
        }
        public override void AddMember(ICSharpMember member)
        {
            ProtectedMembers.Add(member);
        }
    }
}