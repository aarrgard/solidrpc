using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PremisesExpenses {
        /// <summary>
        /// �rshyra kr/m�
        /// </summary>
        [DataMember(Name="yearlyFeePerSquareMeter",EmitDefaultValue=false)]
        public double YearlyFeePerSquareMeter { get; set; }
    
    }
}