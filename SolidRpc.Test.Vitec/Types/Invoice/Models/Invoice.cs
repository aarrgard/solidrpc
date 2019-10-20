using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Invoice.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Invoice {
        /// <summary>
        /// Fakturanummer
        /// </summary>
        [DataMember(Name="number",EmitDefaultValue=false)]
        public string Number { get; set; }
    
    }
}