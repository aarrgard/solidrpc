using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Electricity {
        /// <summary>
        /// F&#246;retag
        /// </summary>
        [DataMember(Name="companie",EmitDefaultValue=false)]
        public string Companie { get; set; }
    
        /// <summary>
        /// Ellevrant&#246;r
        /// </summary>
        [DataMember(Name="distributor",EmitDefaultValue=false)]
        public string Distributor { get; set; }
    
        /// <summary>
        /// Elf&#246;rbrukning KWH /&#229;r
        /// </summary>
        [DataMember(Name="powerConsumptionKWH",EmitDefaultValue=false)]
        public double? PowerConsumptionKWH { get; set; }
    
    }
}