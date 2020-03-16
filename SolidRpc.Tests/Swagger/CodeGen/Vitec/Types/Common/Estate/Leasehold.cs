using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Leasehold {
        /// <summary>
        /// Tomtr&#228;ttsavg&#228;ld kr/&#229;r
        /// </summary>
        [DataMember(Name="leaseholdFee",EmitDefaultValue=false)]
        public double? LeaseholdFee { get; set; }
    
        /// <summary>
        /// L&#246;ptid
        /// </summary>
        [DataMember(Name="leaseholdUntil",EmitDefaultValue=false)]
        public System.DateTimeOffset? LeaseholdUntil { get; set; }
    
    }
}