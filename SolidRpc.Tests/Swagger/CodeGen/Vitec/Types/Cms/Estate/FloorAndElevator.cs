using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Cms.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class FloorAndElevator {
        /// <summary>
        /// V&#229;ning
        /// </summary>
        [DataMember(Name="floor",EmitDefaultValue=false)]
        public double? Floor { get; set; }
    
        /// <summary>
        /// Antal v&#229;ningar
        /// </summary>
        [DataMember(Name="totalNumberFloors",EmitDefaultValue=false)]
        public int? TotalNumberFloors { get; set; }
    
        /// <summary>
        /// Beskrivning av v&#229;ningsplan
        /// </summary>
        [DataMember(Name="floorCommentary",EmitDefaultValue=false)]
        public string FloorCommentary { get; set; }
    
        /// <summary>
        /// Hiss
        /// </summary>
        [DataMember(Name="elevator",EmitDefaultValue=false)]
        public bool? Elevator { get; set; }
    
        /// <summary>
        /// Sammanst&#228;llning balkong/uteplats
        /// </summary>
        [DataMember(Name="descriptionOfElevator",EmitDefaultValue=false)]
        public string DescriptionOfElevator { get; set; }
    
    }
}