using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class RenovationInfo {
        /// <summary>
        /// Renoveringsbehov
        /// </summary>
        [DataMember(Name="renovation",EmitDefaultValue=false)]
        public int Renovation { get; set; }
    
        /// <summary>
        /// Kommentar till renoveringsbehov
        /// </summary>
        [DataMember(Name="comment",EmitDefaultValue=false)]
        public string Comment { get; set; }
    
    }
}