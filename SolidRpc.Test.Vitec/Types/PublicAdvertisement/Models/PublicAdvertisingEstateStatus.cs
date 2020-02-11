using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
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
        /// Url till annonsen p&#229; marknadsplatsens hemsida
        /// </summary>
        [DataMember(Name="advertisementUrl",EmitDefaultValue=false)]
        public string AdvertisementUrl { get; set; }
    
        /// <summary>
        /// En lista av statusinformation som presenteras f&#246;r anv&#228;ndarna.
        /// Anv&#228;nd f&#246;r att ange t ex valideringsfel, varningar eller annan information till anv&#228;ndaren.
        /// L&#229;t det vara null eller skicka inte med om ni inte har n&#229;got s&#228;rskilt att meddela anv&#228;ndaren.
        /// </summary>
        [DataMember(Name="statusInformation",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.PublicAdvertisingEstateStatusInfo> StatusInformation { get; set; }
    
    }
}