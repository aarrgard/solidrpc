using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    public class CSharpInterface : CSharpType, ICSharpInterface
    {
        public CSharpInterface(ICSharpNamespace ns, string name, Type runtimeType) : base(ns, name, runtimeType)
        {
        }
    }
}