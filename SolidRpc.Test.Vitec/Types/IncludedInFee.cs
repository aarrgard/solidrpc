using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class IncludedInFee {
        /// <summary>
        /// V�rme
        /// </summary>
        [DataMember(Name="heat",EmitDefaultValue=false)]
        public bool Heat { get; set; }
    
        /// <summary>
        /// Vatten
        /// </summary>
        [DataMember(Name="water",EmitDefaultValue=false)]
        public bool Water { get; set; }
    
        /// <summary>
        /// Kabel TV
        /// </summary>
        [DataMember(Name="cableTV",EmitDefaultValue=false)]
        public bool CableTV { get; set; }
    
    }
}