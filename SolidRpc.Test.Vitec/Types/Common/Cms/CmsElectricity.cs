using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Common.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsElectricity {
        /// <summary>
        /// N&#228;tbolag
        /// </summary>
        [DataMember(Name="company",EmitDefaultValue=false)]
        public string Company { get; set; }
    
        /// <summary>
        /// Ellevrant&#246;r
        /// </summary>
        [DataMember(Name="distributor",EmitDefaultValue=false)]
        public string Distributor { get; set; }
    
        /// <summary>
        /// Elf&#246;rbrukning KWH /&#229;r
        /// </summary>
        [DataMember(Name="powerConsumptionKWH",EmitDefaultValue=false)]
        public int? PowerConsumptionKWH { get; set; }
    
    }
}