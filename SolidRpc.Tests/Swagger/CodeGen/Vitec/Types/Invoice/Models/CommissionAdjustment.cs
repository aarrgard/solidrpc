using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Invoice.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CommissionAdjustment {
        /// <summary>
        /// Beskrivning av provisionsjustering.
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public string Description { get; set; }
    
        /// <summary>
        /// Belopp
        /// </summary>
        [DataMember(Name="amount",EmitDefaultValue=false)]
        public int Amount { get; set; }
    
        /// <summary>
        /// Momspliktig
        /// </summary>
        [DataMember(Name="vatIncluded",EmitDefaultValue=false)]
        public bool VatIncluded { get; set; }
    
    }
}