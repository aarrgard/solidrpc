using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class HousingExpenses {
        /// <summary>
        /// Driftskostnad per &#229;r
        /// </summary>
        [DataMember(Name="operatingCost",EmitDefaultValue=false)]
        public double? OperatingCost { get; set; }
    
        /// <summary>
        /// Tomtr&#228;ttsavg&#228;ld
        /// </summary>
        [DataMember(Name="plotRent",EmitDefaultValue=false)]
        public double? PlotRent { get; set; }
    
        /// <summary>
        /// Arrende
        /// </summary>
        [DataMember(Name="isLeasehold",EmitDefaultValue=false)]
        public bool? IsLeasehold { get; set; }
    
    }
}