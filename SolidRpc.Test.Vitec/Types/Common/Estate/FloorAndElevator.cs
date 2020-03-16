using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class FloorAndElevator {
        /// <summary>
        /// Beskrivning av hiss
        /// </summary>
        [DataMember(Name="descriptionOfElevator",EmitDefaultValue=false)]
        public string DescriptionOfElevator { get; set; }
    
        /// <summary>
        /// Hiss
        /// </summary>
        [DataMember(Name="elevator",EmitDefaultValue=false)]
        public string Elevator { get; set; }
    
        /// <summary>
        /// Totalt antal v&#229;ningar
        /// </summary>
        [DataMember(Name="totalNumberFloors",EmitDefaultValue=false)]
        public int? TotalNumberFloors { get; set; }
    
        /// <summary>
        /// V&#229;ning
        /// </summary>
        [DataMember(Name="floor",EmitDefaultValue=false)]
        public double? Floor { get; set; }
    
        /// <summary>
        /// Kommentar V&#229;ningsplan
        /// </summary>
        [DataMember(Name="floorCommentary",EmitDefaultValue=false)]
        public string FloorCommentary { get; set; }
    
    }
}