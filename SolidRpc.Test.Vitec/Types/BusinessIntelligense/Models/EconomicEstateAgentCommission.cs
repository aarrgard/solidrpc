using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EconomicEstateAgentCommission {
        /// <summary>
        /// Provisionsv�rdet i kronor
        /// </summary>
        [DataMember(Name="value",EmitDefaultValue=false)]
        public double Value { get; set; }
    
        /// <summary>
        /// Hur m�nga procent av totala provisionen som m�klaren f�r
        /// </summary>
        [DataMember(Name="percentageOfTotal",EmitDefaultValue=false)]
        public double PercentageOfTotal { get; set; }
    
    }
}