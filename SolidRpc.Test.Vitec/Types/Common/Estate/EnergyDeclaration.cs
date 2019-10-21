using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EnergyDeclaration {
        /// <summary>
        /// Utf&#246;rd
        /// </summary>
        [DataMember(Name="completed",EmitDefaultValue=false)]
        public string Completed { get; set; }
    
        /// <summary>
        /// Energideklarationsdatum
        /// </summary>
        [DataMember(Name="energyDeclarationDate",EmitDefaultValue=false)]
        public DateTimeOffset EnergyDeclarationDate { get; set; }
    
        /// <summary>
        /// Energif&#246;rbrukning
        /// </summary>
        [DataMember(Name="energyConsumption",EmitDefaultValue=false)]
        public string EnergyConsumption { get; set; }
    
        /// <summary>
        /// Energiklass
        /// </summary>
        [DataMember(Name="energyClass",EmitDefaultValue=false)]
        public string EnergyClass { get; set; }
    
        /// <summary>
        /// Prim&#228;rtenergital
        /// </summary>
        [DataMember(Name="primaryEnergyNumber",EmitDefaultValue=false)]
        public string PrimaryEnergyNumber { get; set; }
    
    }
}