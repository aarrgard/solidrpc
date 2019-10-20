using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Cms.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class FloorAndElevator {
        /// <summary>
        /// V�ning
        /// </summary>
        [DataMember(Name="floor",EmitDefaultValue=false)]
        public double Floor { get; set; }
    
        /// <summary>
        /// Antal v�ningar
        /// </summary>
        [DataMember(Name="totalNumberFloors",EmitDefaultValue=false)]
        public int TotalNumberFloors { get; set; }
    
        /// <summary>
        /// Beskrivning av v�ningsplan
        /// </summary>
        [DataMember(Name="floorCommentary",EmitDefaultValue=false)]
        public string FloorCommentary { get; set; }
    
        /// <summary>
        /// Hiss
        /// </summary>
        [DataMember(Name="elevator",EmitDefaultValue=false)]
        public bool Elevator { get; set; }
    
        /// <summary>
        /// Sammanst�llning balkong/uteplats
        /// </summary>
        [DataMember(Name="descriptionOfElevator",EmitDefaultValue=false)]
        public string DescriptionOfElevator { get; set; }
    
    }
}