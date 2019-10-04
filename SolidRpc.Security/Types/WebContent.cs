using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// success
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class WebContent {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="content",EmitDefaultValue=false)]
        public Stream Content { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="contentType",EmitDefaultValue=false)]
        public string ContentType { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="charSet",EmitDefaultValue=false)]
        public string CharSet { get; set; }
    
    }
}