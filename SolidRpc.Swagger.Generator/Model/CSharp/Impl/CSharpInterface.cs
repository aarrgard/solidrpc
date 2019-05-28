namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
{
    public class CSharpInterface : CSharpMember, ICSharpInterface
    {
        public CSharpInterface(ICSharpNamespace ns, string name) : base(ns, name)
        {
        }
        public override void AddMember(ICSharpMember member)
        {
            ProtectedMembers.Add(member);
        }
    }
}