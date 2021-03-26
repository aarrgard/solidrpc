using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SolidRpc.Security.Types {
    /// <summary>
    /// success
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class OpenIDKeys {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "keys", EmitDefaultValue = false)]
        public IEnumerable<SolidRpc.Security.Types.OpenIDKey> Keys { get; set; }
    
    }
}