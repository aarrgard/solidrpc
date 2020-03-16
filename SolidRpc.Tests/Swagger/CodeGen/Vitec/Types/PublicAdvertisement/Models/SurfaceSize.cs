using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class SurfaceSize {
        /// <summary>
        /// The value of the size
        /// </summary>
        [DataMember(Name="value",EmitDefaultValue=false)]
        public double? Value { get; set; }
    
        /// <summary>
        /// The unit of the size
        /// </summary>
        [DataMember(Name="unit",EmitDefaultValue=false)]
        public string Unit { get; set; }
    
    }
}