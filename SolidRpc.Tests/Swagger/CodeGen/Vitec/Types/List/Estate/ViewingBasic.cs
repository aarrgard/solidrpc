using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.List.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ViewingBasic {
        /// <summary>
        /// Start datumet f&#246;r visningen
        /// </summary>
        [DataMember(Name="startTime",EmitDefaultValue=false)]
        public DateTimeOffset StartTime { get; set; }
    
        /// <summary>
        /// Slutdatum f&#246;r visningen
        /// </summary>
        [DataMember(Name="endTime",EmitDefaultValue=false)]
        public DateTimeOffset EndTime { get; set; }
    
    }
}