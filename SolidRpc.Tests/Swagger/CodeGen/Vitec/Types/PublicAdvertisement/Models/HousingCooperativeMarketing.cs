using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class HousingCooperativeMarketing {
        /// <summary>
        /// Andel i bostadsf�rening
        /// </summary>
        [DataMember(Name="share",EmitDefaultValue=false)]
        public bool Share { get; set; }
    
        /// <summary>
        /// Andel i bostadsf�rening
        /// </summary>
        [DataMember(Name="shareInHousingSociety",EmitDefaultValue=false)]
        public bool ShareInHousingSociety { get; set; }
    
        /// <summary>
        /// Bostadsr�ttsvilla
        /// </summary>
        [DataMember(Name="isHouse",EmitDefaultValue=false)]
        public bool IsHouse { get; set; }
    
        /// <summary>
        /// Bostadsr�ttsfritidshus
        /// </summary>
        [DataMember(Name="isCottage",EmitDefaultValue=false)]
        public bool IsCottage { get; set; }
    
        /// <summary>
        /// Bostadsr�ttsparhus
        /// </summary>
        [DataMember(Name="isDuplexHouse",EmitDefaultValue=false)]
        public bool IsDuplexHouse { get; set; }
    
        /// <summary>
        /// Bostadsr�ttsradhus
        /// </summary>
        [DataMember(Name="isTerraceHouse",EmitDefaultValue=false)]
        public bool IsTerraceHouse { get; set; }
    
        /// <summary>
        /// Bostadsr�ttskedjehus
        /// </summary>
        [DataMember(Name="isLinkedHouse",EmitDefaultValue=false)]
        public bool IsLinkedHouse { get; set; }
    
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