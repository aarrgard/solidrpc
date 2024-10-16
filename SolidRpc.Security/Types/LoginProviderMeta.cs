using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class LoginProviderMeta {
        /// <summary>
        /// The name of the meta data
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// The content of the meta data
        /// </summary>
        [DataMember(Name="content",EmitDefaultValue=false)]
        public string Content { get; set; }
    
    }
}