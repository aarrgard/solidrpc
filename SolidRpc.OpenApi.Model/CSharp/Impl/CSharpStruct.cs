using System;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Represents a csharp struct
    /// </summary>
    public class CSharpStruct : CSharpType, ICSharpStruct
    {
        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        /// <param name="runtimeType"></param>
        public CSharpStruct(ICSharpNamespace ns, string name, Type runtimeType) : base(ns, name, runtimeType)
        {
        }
    }
}