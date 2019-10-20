using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Urls {
        /// <summary>
        /// Url till bostadsbeskrivning
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public string Description { get; set; }
    
        /// <summary>
        /// Url till bildlista
        /// </summary>
        [DataMember(Name="imageList",EmitDefaultValue=false)]
        public string ImageList { get; set; }
    
    }
}