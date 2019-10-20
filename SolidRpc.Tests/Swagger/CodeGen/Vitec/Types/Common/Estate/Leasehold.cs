using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Leasehold {
        /// <summary>
        /// Tomtr�ttsavg�ld kr/�r
        /// </summary>
        [DataMember(Name="leaseholdFee",EmitDefaultValue=false)]
        public double LeaseholdFee { get; set; }
    
        /// <summary>
        /// L�ptid
        /// </summary>
        [DataMember(Name="leaseholdUntil",EmitDefaultValue=false)]
        public DateTimeOffset LeaseholdUntil { get; set; }
    
    }
}