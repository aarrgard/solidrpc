using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Models.Api {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Range1_Int16 {
        /// <summary>
        /// H&#246;gsta v&#228;rdet i intervallet
        /// </summary>
        [DataMember(Name="maxValue",EmitDefaultValue=false)]
        public int MaxValue { get; set; }
    
        /// <summary>
        /// Minsta v&#228;rdet i intervallet
        /// </summary>
        [DataMember(Name="minValue",EmitDefaultValue=false)]
        public int MinValue { get; set; }
    
    }
}