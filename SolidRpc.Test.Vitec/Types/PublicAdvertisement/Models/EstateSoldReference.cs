using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Models.Api;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EstateSoldReference {
        /// <summary>
        /// Om parter har godk&#228;nt att slutpris f&#229;r visas.
        /// </summary>
        [DataMember(Name="partsApproveFinalPriceVisible",EmitDefaultValue=false)]
        public bool? PartsApproveFinalPriceVisible { get; set; }
    
        /// <summary>
        /// Kontraktsdag
        /// </summary>
        [DataMember(Name="contractDate",EmitDefaultValue=false)]
        public DateTimeOffset? ContractDate { get; set; }
    
        /// <summary>
        /// Slutpris
        /// </summary>
        [DataMember(Name="finalPrice",EmitDefaultValue=false)]
        public MoneyValue FinalPrice { get; set; }
    
    }
}