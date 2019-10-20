using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class HousingCooperativeExpenses {
        /// <summary>
        /// Avgift bostadsr�ttsf�rening
        /// </summary>
        [DataMember(Name="monthlyFee",EmitDefaultValue=false)]
        public double MonthlyFee { get; set; }
    
        /// <summary>
        /// Driftkostnad per �r
        /// </summary>
        [DataMember(Name="operatingCost",EmitDefaultValue=false)]
        public double OperatingCost { get; set; }
    
    }
}