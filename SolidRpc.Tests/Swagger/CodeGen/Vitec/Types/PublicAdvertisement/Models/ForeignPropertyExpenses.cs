using System.CodeDom.Compiler;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Models.Api;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ForeignPropertyExpenses {
        /// <summary>
        /// Avgift bostadsr&#228;ttsf&#246;rening
        /// </summary>
        [DataMember(Name="monthlyFee",EmitDefaultValue=false)]
        public MoneyValue MonthlyFee { get; set; }
    
        /// <summary>
        /// Driftkostnad per &#229;r
        /// </summary>
        [DataMember(Name="operatingCost",EmitDefaultValue=false)]
        public MoneyValue OperatingCost { get; set; }
    
    }
}