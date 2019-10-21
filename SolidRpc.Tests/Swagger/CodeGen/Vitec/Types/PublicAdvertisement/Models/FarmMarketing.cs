using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class FarmMarketing {
        /// <summary>
        /// Avstyckad g&#229;rd
        /// </summary>
        [DataMember(Name="parceledFarm",EmitDefaultValue=false)]
        public bool ParceledFarm { get; set; }
    
        /// <summary>
        /// Jordbruk
        /// </summary>
        [DataMember(Name="isAgriculture",EmitDefaultValue=false)]
        public bool IsAgriculture { get; set; }
    
        /// <summary>
        /// Skogsbruk
        /// </summary>
        [DataMember(Name="isForestFarm",EmitDefaultValue=false)]
        public bool IsForestFarm { get; set; }
    
        /// <summary>
        /// H&#228;stg&#229;rd
        /// </summary>
        [DataMember(Name="isHorseFarm",EmitDefaultValue=false)]
        public bool IsHorseFarm { get; set; }
    
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