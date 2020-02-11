using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Api.Connect {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CdnImageReference {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="url",EmitDefaultValue=false)]
        public string Url { get; set; }
    
    }
}