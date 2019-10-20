using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Represents a using directive
    /// </summary>
    public class CSharpUsing : CSharpMember, ICSharpUsing
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="ns"></param>
        /// <param name="nsName"></param>
        public CSharpUsing(ICSharpMember parent, string ns, string nsName = null) : base(parent, ns)
        {
            NsName = nsName;
        }

        /// <summary>
        /// The namespace name. If set we will add a "NsName = NS.NsName" to the emitted code
        /// </summary>
        public string NsName { get; }

        /// <summary>
        /// Emits the using directive
        /// </summary>
        /// <param name="codeWriter"></param>
        public override void WriteCode(ICodeWriter codeWriter)
        {
            if(string.IsNullOrEmpty(NsName))
            {
                codeWriter.Emit($"using {Name};{codeWriter.NewLine}");
            }
            else
            {
                codeWriter.Emit($"using {NsName} = {Name}.{NsName};{codeWriter.NewLine}");
            }
        }
    }
}
