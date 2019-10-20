using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Interest.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Interval {
        /// <summary>
        /// Minimumv�rde
        /// </summary>
        [DataMember(Name="min",EmitDefaultValue=false)]
        public double Min { get; set; }
    
        /// <summary>
        /// Maximumv�rde
        /// </summary>
        [DataMember(Name="max",EmitDefaultValue=false)]
        public double Max { get; set; }
    
    }
}