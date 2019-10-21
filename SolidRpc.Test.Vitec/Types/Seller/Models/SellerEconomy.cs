using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Seller.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class SellerEconomy {
        /// <summary>
        /// S&#228;ljarens bank
        /// </summary>
        [DataMember(Name="bank",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Seller.Models.Bank Bank { get; set; }
    
    }
}