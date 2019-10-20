using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.IncreasedRequirement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class IncreasedRequirementCollection {
        /// <summary>
        /// Kundnummer
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Ut�kade krav
        /// </summary>
        [DataMember(Name="increasedRequirements",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.IncreasedRequirement.Models.IncreasedRequirement> IncreasedRequirements { get; set; }
    
    }
}