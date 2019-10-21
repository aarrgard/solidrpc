using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Area.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PublicAreaCritera {
        /// <summary>
        /// Omr&#229;dets id
        /// </summary>
        [DataMember(Name="areaId",EmitDefaultValue=false)]
        public string AreaId { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
    }
}