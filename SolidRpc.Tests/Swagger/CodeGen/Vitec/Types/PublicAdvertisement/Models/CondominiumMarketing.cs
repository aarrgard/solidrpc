using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CondominiumMarketing {
        /// <summary>
        /// Nyproduktion
        /// </summary>
        [DataMember(Name="isNewHome",EmitDefaultValue=false)]
        public bool? IsNewHome { get; set; }
    
        /// <summary>
        /// Byteskrav
        /// </summary>
        [DataMember(Name="swapDemanded",EmitDefaultValue=false)]
        public bool? SwapDemanded { get; set; }
    
        /// <summary>
        /// Kommande
        /// </summary>
        [DataMember(Name="isFutureSale",EmitDefaultValue=false)]
        public bool? IsFutureSale { get; set; }
    
        /// <summary>
        /// Snart till salu
        /// </summary>
        [DataMember(Name="isSoonForSale",EmitDefaultValue=false)]
        public bool? IsSoonForSale { get; set; }
    
    }
}