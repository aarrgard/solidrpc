using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Represents a csharp enum value
    /// </summary>
    public class CSharpEnumValue : CSharpMember, ICSharpEnumValue
    {
        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="e"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public CSharpEnumValue(ICSharpEnum e, string name, int? value) : base(e, name)
        {
            Value = value;
        }

        /// <summary>
        /// The value of the enum
        /// </summary>
        public int? Value { get; }

        /// <summary>
        /// Emits the code
        /// </summary>
        /// <param name="codeWriter"></param>
        public override void WriteCode(ICodeWriter codeWriter)
        {
            codeWriter.Emit(Name);
            if(Value != null)
            {
                codeWriter.Emit("=");
                codeWriter.Emit(Value.ToString());
            }
            codeWriter.Emit(";");
        }
    }
}