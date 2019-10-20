using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Texts {
        /// <summary>
        /// Kort s�ljande beskrivning
        /// </summary>
        [DataMember(Name="shortSaleDescription",EmitDefaultValue=false)]
        public string ShortSaleDescription { get; set; }
    
        /// <summary>
        /// S�ljande beskrivning
        /// </summary>
        [DataMember(Name="saleDescription",EmitDefaultValue=false)]
        public string SaleDescription { get; set; }
    
        /// <summary>
        /// S�ljfras
        /// </summary>
        [DataMember(Name="salePhrase",EmitDefaultValue=false)]
        public string SalePhrase { get; set; }
    
        /// <summary>
        /// S�ljrubrik
        /// </summary>
        [DataMember(Name="saleHeading",EmitDefaultValue=false)]
        public string SaleHeading { get; set; }
    
    }
}