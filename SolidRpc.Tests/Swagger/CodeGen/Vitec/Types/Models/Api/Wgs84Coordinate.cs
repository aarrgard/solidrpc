using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Models.Api {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Wgs84Coordinate {
        /// <summary>
        /// Longitud
        /// </summary>
        [DataMember(Name="longitude",EmitDefaultValue=false)]
        public double? Longitude { get; set; }
    
        /// <summary>
        /// Latitud
        /// </summary>
        [DataMember(Name="latitude",EmitDefaultValue=false)]
        public double? Latitude { get; set; }
    
    }
}