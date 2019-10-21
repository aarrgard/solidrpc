using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PremisesSurface {
        /// <summary>
        /// Namn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Typ av yta
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
        /// <summary>
        /// Storlek
        /// </summary>
        [DataMember(Name="size",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.SurfaceSize Size { get; set; }
    
        /// <summary>
        /// M&#229;nadshyra
        /// </summary>
        [DataMember(Name="yearlyFee",EmitDefaultValue=false)]
        public double YearlyFee { get; set; }
    
    }
}