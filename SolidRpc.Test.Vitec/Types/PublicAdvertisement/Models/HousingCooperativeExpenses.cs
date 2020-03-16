using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class HousingCooperativeExpenses {
        /// <summary>
        /// Avgift bostadsr&#228;ttsf&#246;rening
        /// </summary>
        [DataMember(Name="monthlyFee",EmitDefaultValue=false)]
        public double? MonthlyFee { get; set; }
    
        /// <summary>
        /// Driftkostnad per &#229;r
        /// </summary>
        [DataMember(Name="operatingCost",EmitDefaultValue=false)]
        public double? OperatingCost { get; set; }
    
    }
}