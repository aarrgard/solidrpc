using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EconomicEstateAgentChangeEntry {
        /// <summary>
        /// M&#228;klaren &#228;ndrades vid detta datum
        /// </summary>
        [DataMember(Name="changedAt",EmitDefaultValue=false)]
        public DateTimeOffset ChangedAt { get; set; }
    
        /// <summary>
        /// Provisionsv&#228;rdet i kronor
        /// </summary>
        [DataMember(Name="commission",EmitDefaultValue=false)]
        public double Commission { get; set; }
    
        /// <summary>
        /// Hur m&#229;nga procent av totala provisionen som m&#228;klaren f&#229;r
        /// </summary>
        [DataMember(Name="percentageOfTotal",EmitDefaultValue=false)]
        public double PercentageOfTotal { get; set; }
    
    }
}