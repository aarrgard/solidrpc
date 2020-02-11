using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ForeignLanguage {
        /// <summary>
        /// Spr&#229;kkod enligt ISO-639-1.
        /// https://sv.wikipedia.org/wiki/ISO_639
        /// </summary>
        [DataMember(Name="languageCode",EmitDefaultValue=false)]
        public string LanguageCode { get; set; }
    
        /// <summary>
        /// Kort s&#228;ljande beskrivning
        /// </summary>
        [DataMember(Name="shortSaleDescription",EmitDefaultValue=false)]
        public string ShortSaleDescription { get; set; }
    
        /// <summary>
        /// S&#228;ljande beskrivning
        /// </summary>
        [DataMember(Name="saleDescription",EmitDefaultValue=false)]
        public string SaleDescription { get; set; }
    
        /// <summary>
        /// S&#228;ljfras
        /// </summary>
        [DataMember(Name="salePhrase",EmitDefaultValue=false)]
        public string SalePhrase { get; set; }
    
        /// <summary>
        /// S&#228;ljrubrik
        /// </summary>
        [DataMember(Name="saleHeading",EmitDefaultValue=false)]
        public string SaleHeading { get; set; }
    
    }
}