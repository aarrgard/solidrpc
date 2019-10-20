using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.FarmInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AssessValue {
        /// <summary>
        /// V�rderingsenhet
        /// </summary>
        [DataMember(Name="evaluationUnit",EmitDefaultValue=false)]
        public string EvaluationUnit { get; set; }
    
        /// <summary>
        /// Taxeringsv�rde
        /// </summary>
        [DataMember(Name="value",EmitDefaultValue=false)]
        public double Value { get; set; }
    
        /// <summary>
        /// V�rde�r
        /// </summary>
        [DataMember(Name="valueYear",EmitDefaultValue=false)]
        public string ValueYear { get; set; }
    
    }
}