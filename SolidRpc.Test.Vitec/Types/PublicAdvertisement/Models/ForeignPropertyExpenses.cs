using System.CodeDom.Compiler;
using SolidRpc.Test.Vitec.Types.Models.Api;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ForeignPropertyExpenses {
        /// <summary>
        /// Avgift bostadsr�ttsf�rening
        /// </summary>
        [DataMember(Name="monthlyFee",EmitDefaultValue=false)]
        public MoneyValue MonthlyFee { get; set; }
    
        /// <summary>
        /// Driftkostnad per �r
        /// </summary>
        [DataMember(Name="operatingCost",EmitDefaultValue=false)]
        public MoneyValue OperatingCost { get; set; }
    
    }
}