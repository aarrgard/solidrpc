using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Cms.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Description {
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
        /// &#214;vrigt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
    }
}