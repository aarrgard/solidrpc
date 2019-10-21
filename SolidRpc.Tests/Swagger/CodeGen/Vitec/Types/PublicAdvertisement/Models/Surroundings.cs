using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Surroundings {
        /// <summary>
        /// N&#228;rservice
        /// </summary>
        [DataMember(Name="service",EmitDefaultValue=false)]
        public string Service { get; set; }
    
        /// <summary>
        /// Kommunikation
        /// </summary>
        [DataMember(Name="communication",EmitDefaultValue=false)]
        public string Communication { get; set; }
    
        /// <summary>
        /// Allm&#228;nt om omr&#229;det
        /// </summary>
        [DataMember(Name="generalAboutArea",EmitDefaultValue=false)]
        public string GeneralAboutArea { get; set; }
    
        /// <summary>
        /// Parkering
        /// </summary>
        [DataMember(Name="parking",EmitDefaultValue=false)]
        public string Parking { get; set; }
    
    }
}