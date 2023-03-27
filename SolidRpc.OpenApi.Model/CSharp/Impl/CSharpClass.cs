using System;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Represents a csharp class
    /// </summary>
    public class CSharpClass : CSharpType, ICSharpClass
    {
        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        /// <param name="runtimeType"></param>
        public CSharpClass(ICSharpMember ns, string name, Type runtimeType) : base(ns, name, runtimeType)
        {
        }
    }
}