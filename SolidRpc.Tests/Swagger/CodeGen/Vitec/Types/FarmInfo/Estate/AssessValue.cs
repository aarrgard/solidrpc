using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.FarmInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AssessValue {
        /// <summary>
        /// V&#228;rderingsenhet
        /// </summary>
        [DataMember(Name="evaluationUnit",EmitDefaultValue=false)]
        public string EvaluationUnit { get; set; }
    
        /// <summary>
        /// Taxeringsv&#228;rde
        /// </summary>
        [DataMember(Name="value",EmitDefaultValue=false)]
        public double? Value { get; set; }
    
        /// <summary>
        /// V&#228;rde&#229;r
        /// </summary>
        [DataMember(Name="valueYear",EmitDefaultValue=false)]
        public string ValueYear { get; set; }
    
    }
}