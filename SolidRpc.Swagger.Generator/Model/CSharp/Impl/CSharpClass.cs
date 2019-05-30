using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
{
    public class CSharpClass : CSharpMember, ICSharpClass
    {
        public CSharpClass(ICSharpNamespace ns, string name, Type runtimeType) : base(ns, name)
        {
            RuntimeType = runtimeType;
        }

        public Type RuntimeType { get; }

        public override void AddMember(ICSharpMember member)
        {
            ProtectedMembers.Add(member);
        }
    }
}