using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ForeignProperty.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ForeignPropertyDescription {
        /// <summary>
        /// S&#228;ljfras
        /// </summary>
        [DataMember(Name="sellPhrase",EmitDefaultValue=false)]
        public string SellPhrase { get; set; }
    
        /// <summary>
        /// S&#228;ljande rubrik
        /// </summary>
        [DataMember(Name="sellingHeading",EmitDefaultValue=false)]
        public string SellingHeading { get; set; }
    
        /// <summary>
        /// Kort s&#228;ljande beskrivning
        /// </summary>
        [DataMember(Name="shortSellingDescription",EmitDefaultValue=false)]
        public string ShortSellingDescription { get; set; }
    
        /// <summary>
        /// L&#229;ng s&#228;ljande beskrivning
        /// </summary>
        [DataMember(Name="longSellingDescription",EmitDefaultValue=false)]
        public string LongSellingDescription { get; set; }
    
        /// <summary>
        /// Allm&#228;n interi&#246;rbeskrivning
        /// </summary>
        [DataMember(Name="interiorDescription",EmitDefaultValue=false)]
        public string InteriorDescription { get; set; }
    
        /// <summary>
        /// Spr&#229;k Koder fr&#229;n ISO 639-1
        /// </summary>
        [DataMember(Name="language",EmitDefaultValue=false)]
        public string Language { get; set; }
    
    }
}