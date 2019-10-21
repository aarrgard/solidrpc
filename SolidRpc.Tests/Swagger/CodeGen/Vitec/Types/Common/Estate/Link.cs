using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Link {
        /// <summary>
        /// L&#228;nkar
        /// </summary>
        [DataMember(Name="links",EmitDefaultValue=false)]
        public IEnumerable<string> Links { get; set; }
    
        /// <summary>
        /// Text
        /// </summary>
        [DataMember(Name="text",EmitDefaultValue=false)]
        public string Text { get; set; }
    
        /// <summary>
        /// Kategori
        /// </summary>
        [DataMember(Name="category",EmitDefaultValue=false)]
        public string Category { get; set; }
    
    }
}