using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class HouseMarketing {
        /// <summary>
        /// Parhus
        /// </summary>
        [DataMember(Name="isDuplexHouse",EmitDefaultValue=false)]
        public bool IsDuplexHouse { get; set; }
    
        /// <summary>
        /// Radhus
        /// </summary>
        [DataMember(Name="isTerraceHouse",EmitDefaultValue=false)]
        public bool IsTerraceHouse { get; set; }
    
        /// <summary>
        /// Kedjehus
        /// </summary>
        [DataMember(Name="isLinkedHouse",EmitDefaultValue=false)]
        public bool IsLinkedHouse { get; set; }
    
        /// <summary>
        /// Friliggande
        /// </summary>
        [DataMember(Name="isDetachedHouse",EmitDefaultValue=false)]
        public bool IsDetachedHouse { get; set; }
    
        /// <summary>
        /// Lantbruk
        /// </summary>
        [DataMember(Name="isFarm",EmitDefaultValue=false)]
        public bool IsFarm { get; set; }
    
        /// <summary>
        /// �garl�genhet
        /// </summary>
        [DataMember(Name="isCondominium",EmitDefaultValue=false)]
        public bool IsCondominium { get; set; }
    
        /// <summary>
        /// Fritidshus
        /// </summary>
        [DataMember(Name="isCottage",EmitDefaultValue=false)]
        public bool IsCottage { get; set; }
    
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