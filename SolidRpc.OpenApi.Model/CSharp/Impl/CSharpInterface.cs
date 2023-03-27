using System;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Represents a csharp interface
    /// </summary>
    public class CSharpInterface : CSharpType, ICSharpInterface
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        /// <param name="runtimeType"></param>
        public CSharpInterface(ICSharpMember ns, string name, Type runtimeType) : base(ns, name, runtimeType)
        {
        }
    }
}