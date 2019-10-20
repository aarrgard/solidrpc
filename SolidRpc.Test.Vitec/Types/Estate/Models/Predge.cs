using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Estate.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Predge {
        /// <summary>
        /// Pantsatt
        /// </summary>
        [DataMember(Name="pawned",EmitDefaultValue=false)]
        public bool Pawned { get; set; }
    
        /// <summary>
        /// Uppgiften kontrollerad
        /// </summary>
        [DataMember(Name="taskControlled",EmitDefaultValue=false)]
        public DateTimeOffset TaskControlled { get; set; }
    
        /// <summary>
        /// Anteckningar
        /// </summary>
        [DataMember(Name="notes",EmitDefaultValue=false)]
        public string Notes { get; set; }
    
    }
}