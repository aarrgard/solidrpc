using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Models.Api {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class MoneyValue {
        /// <summary>
        /// V�rde
        /// </summary>
        [DataMember(Name="value",EmitDefaultValue=false)]
        public double Value { get; set; }
    
        /// <summary>
        /// Valuta (SEK, EURO, USD, osv)
        /// </summary>
        [DataMember(Name="currency",EmitDefaultValue=false)]
        public string Currency { get; set; }
    
    }
}