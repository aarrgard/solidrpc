using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Models.Api {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Polygon {
        /// <summary>
        /// Koordinater
        /// </summary>
        [DataMember(Name="coordinates",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Models.Api.Coordinate> Coordinates { get; set; }
    
    }
}