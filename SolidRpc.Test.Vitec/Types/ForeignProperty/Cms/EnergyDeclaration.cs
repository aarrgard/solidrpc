using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.ForeignProperty.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EnergyDeclaration {
        /// <summary>
        /// Utf&#246;rd
        /// </summary>
        [DataMember(Name="status",EmitDefaultValue=false)]
        public string Status { get; set; }
    
        /// <summary>
        /// Energideklarationsdatum
        /// </summary>
        [DataMember(Name="energyDeclarationDate",EmitDefaultValue=false)]
        public DateTimeOffset EnergyDeclarationDate { get; set; }
    
        /// <summary>
        /// Energif&#246;rbrukning kr/kvm/&#229;r
        /// </summary>
        [DataMember(Name="energyConsumption",EmitDefaultValue=false)]
        public double EnergyConsumption { get; set; }
    
        /// <summary>
        /// Energiklass
        /// </summary>
        [DataMember(Name="energyClass",EmitDefaultValue=false)]
        public string EnergyClass { get; set; }
    
    }
}