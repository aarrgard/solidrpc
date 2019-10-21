using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CommercialPropertyBuilding {
        /// <summary>
        /// Namn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Bygg&#229;r
        /// </summary>
        [DataMember(Name="yearBuilt",EmitDefaultValue=false)]
        public int YearBuilt { get; set; }
    
        /// <summary>
        /// Antal v&#229;ningsplan
        /// </summary>
        [DataMember(Name="numberOfFloors",EmitDefaultValue=false)]
        public double NumberOfFloors { get; set; }
    
        /// <summary>
        /// Information om hiss
        /// </summary>
        [DataMember(Name="elevator",EmitDefaultValue=false)]
        public string Elevator { get; set; }
    
    }
}