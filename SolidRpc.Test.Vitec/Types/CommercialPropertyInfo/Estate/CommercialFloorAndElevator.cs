using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CommercialFloorAndElevator {
        /// <summary>
        /// Antal v&#229;ningar
        /// </summary>
        [DataMember(Name="numberOfFloors",EmitDefaultValue=false)]
        public int NumberOfFloors { get; set; }
    
        /// <summary>
        /// Hiss (Ja, Nej eller ok&#228;nt)
        /// </summary>
        [DataMember(Name="elevator",EmitDefaultValue=false)]
        public string Elevator { get; set; }
    
        /// <summary>
        /// &#214;vriga hissuppgifter
        /// </summary>
        [DataMember(Name="descriptionOfElevator",EmitDefaultValue=false)]
        public string DescriptionOfElevator { get; set; }
    
    }
}