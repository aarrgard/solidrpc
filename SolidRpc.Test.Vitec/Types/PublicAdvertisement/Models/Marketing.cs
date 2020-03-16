using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Marketing {
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