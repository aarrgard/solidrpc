using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Represents a csharp enum
    /// </summary>
    public class CSharpEnum : CSharpType, ICSharpEnum
    {
        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        /// <param name="runtimeType"></param>
        public CSharpEnum(ICSharpNamespace ns, string name, Type runtimeType) : base(ns, name, runtimeType)
        {

        }

        /// <summary>
        /// Returns the enum values
        /// </summary>
        public IEnumerable<ICSharpEnumValue> EnumValues => Members.OfType<ICSharpEnumValue>();
    }
}