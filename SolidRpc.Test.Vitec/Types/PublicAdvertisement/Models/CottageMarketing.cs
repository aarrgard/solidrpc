using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CottageMarketing {
        /// <summary>
        /// Vinterbonat
        /// </summary>
        [DataMember(Name="allYear",EmitDefaultValue=false)]
        public bool AllYear { get; set; }
    
        /// <summary>
        /// Lantbruk
        /// </summary>
        [DataMember(Name="isFarm",EmitDefaultValue=false)]
        public bool IsFarm { get; set; }
    
        /// <summary>
        /// Bostadsr�ttsfritidshus
        /// </summary>
        [DataMember(Name="isHousingCooperative",EmitDefaultValue=false)]
        public bool IsHousingCooperative { get; set; }
    
        /// <summary>
        /// �vrig bostad
        /// </summary>
        [DataMember(Name="isOtherType",EmitDefaultValue=false)]
        public bool IsOtherType { get; set; }
    
        /// <summary>
        /// Nyproduktion
        /// </summary>
        [DataMember(Name="isNewHome",EmitDefaultValue=false)]
        public bool IsNewHome { get; set; }
    
        /// <summary>
        /// Byteskrav
        /// </summary>
        [DataMember(Name="swapDemanded",EmitDefaultValue=false)]
        public bool SwapDemanded { get; set; }
    
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