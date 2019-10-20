using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EnergyDeclaration {
        /// <summary>
        /// Energiprestanda
        /// </summary>
        [DataMember(Name="energyPerformance",EmitDefaultValue=false)]
        public double EnergyPerformance { get; set; }
    
        /// <summary>
        /// Energiklass
        /// </summary>
        [DataMember(Name="energyClass",EmitDefaultValue=false)]
        public string EnergyClass { get; set; }
    
    }
}