using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CondominiumInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CarSpaceOtherBuildings {
        /// <summary>
        /// Bilplats
        /// </summary>
        [DataMember(Name="carSpace",EmitDefaultValue=false)]
        public string CarSpace { get; set; }
    
        /// <summary>
        /// �vriga byggnader
        /// </summary>
        [DataMember(Name="otherBuildings",EmitDefaultValue=false)]
        public string OtherBuildings { get; set; }
    
    }
}