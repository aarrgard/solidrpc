using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ForeignPropertyMarketing {
        /// <summary>
        /// Nyproduktion
        /// </summary>
        [DataMember(Name="isNewHome",EmitDefaultValue=false)]
        public bool IsNewHome { get; set; }
    
        /// <summary>
        /// L�genhet
        /// </summary>
        [DataMember(Name="isAppartment",EmitDefaultValue=false)]
        public bool IsAppartment { get; set; }
    
        /// <summary>
        /// Radhus
        /// </summary>
        [DataMember(Name="isTerraceHouse",EmitDefaultValue=false)]
        public bool IsTerraceHouse { get; set; }
    
        /// <summary>
        /// Tomt
        /// </summary>
        [DataMember(Name="isPlot",EmitDefaultValue=false)]
        public bool IsPlot { get; set; }
    
        /// <summary>
        /// Villa
        /// </summary>
        [DataMember(Name="isHouse",EmitDefaultValue=false)]
        public bool IsHouse { get; set; }
    
        /// <summary>
        /// �vrig bostad
        /// </summary>
        [DataMember(Name="isOtherType",EmitDefaultValue=false)]
        public bool IsOtherType { get; set; }
    
        /// <summary>
        /// Kommande
        /// </summary>
        [DataMember(Name="isFutureSale",EmitDefaultValue=false)]
        public bool IsFutureSale { get; set; }
    
        /// <summary>
        /// Snart till salu
        /// </summary>
        [DataMember(Name="isSoonForSale",EmitDefaultValue=false)]
        public bool IsSoonForSale { get; set; }
    
    }
}