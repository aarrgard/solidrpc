using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CondominiumExpenses {
        /// <summary>
        /// �rsavgift till samf�llighet
        /// </summary>
        [DataMember(Name="yearlyCommunityFee",EmitDefaultValue=false)]
        public double YearlyCommunityFee { get; set; }
    
        /// <summary>
        /// Driftkostnad per �r
        /// </summary>
        [DataMember(Name="operatingCost",EmitDefaultValue=false)]
        public double OperatingCost { get; set; }
    
    }
}