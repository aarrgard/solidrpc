using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CommercialPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class OwnInput {
        /// <summary>
        /// Rubrik
        /// </summary>
        [DataMember(Name="heading",EmitDefaultValue=false)]
        public string Heading { get; set; }
    
        /// <summary>
        /// Belopp
        /// </summary>
        [DataMember(Name="amount",EmitDefaultValue=false)]
        public double Amount { get; set; }
    
    }
}