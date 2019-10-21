using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Villavardet.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Agent {
        /// <summary>
        /// Namn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// M&#228;klarkontor
        /// </summary>
        [DataMember(Name="office",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Villavardet.Models.Office Office { get; set; }
    
    }
}