using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PublicAdvertisingEstateStatus {
        /// <summary>
        /// Annnonsid hos marknadsplatsen
        /// </summary>
        [DataMember(Name="advertisementId",EmitDefaultValue=false)]
        public string AdvertisementId { get; set; }
    
        /// <summary>
        /// Url till annonsen p� marknadsplatsens hemsida
        /// </summary>
        [DataMember(Name="advertisementUrl",EmitDefaultValue=false)]
        public string AdvertisementUrl { get; set; }
    
    }
}