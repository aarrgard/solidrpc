using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EconomicEstateAgentChangeEntry {
        /// <summary>
        /// M�klaren �ndrades vid detta datum
        /// </summary>
        [DataMember(Name="changedAt",EmitDefaultValue=false)]
        public DateTimeOffset ChangedAt { get; set; }
    
        /// <summary>
        /// Provisionsv�rdet i kronor
        /// </summary>
        [DataMember(Name="commission",EmitDefaultValue=false)]
        public double Commission { get; set; }
    
        /// <summary>
        /// Hur m�nga procent av totala provisionen som m�klaren f�r
        /// </summary>
        [DataMember(Name="percentageOfTotal",EmitDefaultValue=false)]
        public double PercentageOfTotal { get; set; }
    
    }
}