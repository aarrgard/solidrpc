using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Description {
        /// <summary>
        /// S�ljfras
        /// </summary>
        [DataMember(Name="sellPhrase",EmitDefaultValue=false)]
        public string SellPhrase { get; set; }
    
        /// <summary>
        /// Allm�nt
        /// </summary>
        [DataMember(Name="generally",EmitDefaultValue=false)]
        public string Generally { get; set; }
    
        /// <summary>
        /// S�ljande rubrik
        /// </summary>
        [DataMember(Name="sellingHeading",EmitDefaultValue=false)]
        public string SellingHeading { get; set; }
    
        /// <summary>
        /// Kort s�ljande beskrivning
        /// </summary>
        [DataMember(Name="shortSellingDescription",EmitDefaultValue=false)]
        public string ShortSellingDescription { get; set; }
    
        /// <summary>
        /// L�ng s�ljande beskrivning
        /// </summary>
        [DataMember(Name="longSellingDescription",EmitDefaultValue=false)]
        public string LongSellingDescription { get; set; }
    
        /// <summary>
        /// V�gbeskrivning
        /// </summary>
        [DataMember(Name="wayDescription",EmitDefaultValue=false)]
        public string WayDescription { get; set; }
    
        /// <summary>
        /// Spr�k
        /// </summary>
        [DataMember(Name="language",EmitDefaultValue=false)]
        public string Language { get; set; }
    
        /// <summary>
        /// �vrigt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
    }
}