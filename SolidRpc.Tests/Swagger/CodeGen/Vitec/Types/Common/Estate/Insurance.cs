using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Insurance {
        /// <summary>
        /// Bolag
        /// </summary>
        [DataMember(Name="company",EmitDefaultValue=false)]
        public string Company { get; set; }
    
        /// <summary>
        /// Belopp
        /// </summary>
        [DataMember(Name="amount",EmitDefaultValue=false)]
        public double Amount { get; set; }
    
        /// <summary>
        /// Fullv�rdesf�rs�krad
        /// </summary>
        [DataMember(Name="fullValue",EmitDefaultValue=false)]
        public string FullValue { get; set; }
    
    }
}