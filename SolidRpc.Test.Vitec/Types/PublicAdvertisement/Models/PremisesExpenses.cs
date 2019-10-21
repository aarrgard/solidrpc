using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PremisesExpenses {
        /// <summary>
        /// &#197;rshyra kr/m&#178;
        /// </summary>
        [DataMember(Name="yearlyFeePerSquareMeter",EmitDefaultValue=false)]
        public double YearlyFeePerSquareMeter { get; set; }
    
    }
}