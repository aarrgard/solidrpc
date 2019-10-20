using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ForeignPropertyInfo.Estate {
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
        /// Totalt antal v�ningar
        /// </summary>
        [DataMember(Name="totalNumberFloors",EmitDefaultValue=false)]
        public int TotalNumberFloors { get; set; }
    
        /// <summary>
        /// V�ning
        /// </summary>
        [DataMember(Name="floor",EmitDefaultValue=false)]
        public double Floor { get; set; }
    
    }
}