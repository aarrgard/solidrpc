using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CommercialFloorAndElevator {
        /// <summary>
        /// Antal v�ningar
        /// </summary>
        [DataMember(Name="numberOfFloors",EmitDefaultValue=false)]
        public int NumberOfFloors { get; set; }
    
        /// <summary>
        /// Hiss (Ja, Nej eller ok�nt)
        /// </summary>
        [DataMember(Name="elevator",EmitDefaultValue=false)]
        public string Elevator { get; set; }
    
        /// <summary>
        /// �vriga hissuppgifter
        /// </summary>
        [DataMember(Name="descriptionOfElevator",EmitDefaultValue=false)]
        public string DescriptionOfElevator { get; set; }
    
    }
}