using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Models.Api {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Coordinate {
        /// <summary>
        /// Longitud
        /// </summary>
        [DataMember(Name="longitud",EmitDefaultValue=false)]
        public double? Longitud { get; set; }
    
        /// <summary>
        /// Latitud
        /// </summary>
        [DataMember(Name="latitud",EmitDefaultValue=false)]
        public double? Latitud { get; set; }
    
    }
}