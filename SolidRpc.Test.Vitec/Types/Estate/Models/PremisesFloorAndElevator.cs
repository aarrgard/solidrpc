using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Estate.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PremisesFloorAndElevator {
        /// <summary>
        /// Beskrivning av hiss
        /// </summary>
        [DataMember(Name="descriptionOfElevator",EmitDefaultValue=false)]
        public string DescriptionOfElevator { get; set; }
    
        /// <summary>
        /// Hiss
        /// </summary>
        [DataMember(Name="elevator",EmitDefaultValue=false)]
        public bool Elevator { get; set; }
    
        /// <summary>
        /// V&#229;ningar
        /// </summary>
        [DataMember(Name="floors",EmitDefaultValue=false)]
        public string Floors { get; set; }
    
    }
}