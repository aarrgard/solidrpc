using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
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
        /// Allm&#228;nt
        /// </summary>
        [DataMember(Name="generally",EmitDefaultValue=false)]
        public string Generally { get; set; }
    
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
        /// V&#228;gbeskrivning
        /// </summary>
        [DataMember(Name="wayDescription",EmitDefaultValue=false)]
        public string WayDescription { get; set; }
    
        /// <summary>
        /// Spr&#229;k
        /// </summary>
        [DataMember(Name="language",EmitDefaultValue=false)]
        public string Language { get; set; }
    
        /// <summary>
        /// &#214;vrigt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
    }
}