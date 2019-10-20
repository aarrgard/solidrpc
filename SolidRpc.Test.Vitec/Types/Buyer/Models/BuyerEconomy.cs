using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Buyer.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class BuyerEconomy {
        /// <summary>
        /// K�parens bank
        /// </summary>
        [DataMember(Name="bank",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Buyer.Models.Bank Bank { get; set; }
    
        /// <summary>
        /// K�parens f�rs�kringsbolag
        /// </summary>
        [DataMember(Name="insuranceCompany",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Buyer.Models.InsuranceCompany InsuranceCompany { get; set; }
    
    }
}