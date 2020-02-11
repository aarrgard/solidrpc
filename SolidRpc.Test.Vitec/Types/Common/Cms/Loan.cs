using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Common.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Loan {
        /// <summary>
        /// L&#229;ngivare
        /// </summary>
        [DataMember(Name="lender",EmitDefaultValue=false)]
        public string Lender { get; set; }
    
        /// <summary>
        /// L&#229;nenummer
        /// </summary>
        [DataMember(Name="loanNumber",EmitDefaultValue=false)]
        public string LoanNumber { get; set; }
    
        /// <summary>
        /// Belopp
        /// </summary>
        [DataMember(Name="amount",EmitDefaultValue=false)]
        public double Amount { get; set; }
    
    }
}