using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EconomicEstateAgentCommission {
        /// <summary>
        /// Provisionsv&#228;rdet i kronor
        /// </summary>
        [DataMember(Name="value",EmitDefaultValue=false)]
        public double? Value { get; set; }
    
        /// <summary>
        /// Provisionsv&#228;rdet i utl&#228;ndsk valuta, g&#228;ller endast utlandsbost&#228;der.
        /// </summary>
        [DataMember(Name="valueForeign",EmitDefaultValue=false)]
        public double? ValueForeign { get; set; }
    
        /// <summary>
        /// Hur m&#229;nga procent av totala provisionen som m&#228;klaren f&#229;r
        /// </summary>
        [DataMember(Name="percentageOfTotal",EmitDefaultValue=false)]
        public double? PercentageOfTotal { get; set; }
    
    }
}