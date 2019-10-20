using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Seller.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Bank {
        /// <summary>
        /// Banknamn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Clearingnummer
        /// </summary>
        [DataMember(Name="clearingNumber",EmitDefaultValue=false)]
        public string ClearingNumber { get; set; }
    
        /// <summary>
        /// Kontonummer
        /// </summary>
        [DataMember(Name="accountNumber",EmitDefaultValue=false)]
        public string AccountNumber { get; set; }
    
    }
}