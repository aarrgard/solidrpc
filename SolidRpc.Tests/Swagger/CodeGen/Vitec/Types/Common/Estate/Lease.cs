using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Lease {
        /// <summary>
        /// Arrende kr/&#229;r
        /// </summary>
        [DataMember(Name="leaseFee",EmitDefaultValue=false)]
        public double? LeaseFee { get; set; }
    
        /// <summary>
        /// Kommentar
        /// </summary>
        [DataMember(Name="leaseFeeComment",EmitDefaultValue=false)]
        public string LeaseFeeComment { get; set; }
    
        /// <summary>
        /// L&#246;ptid
        /// </summary>
        [DataMember(Name="leaseUntil",EmitDefaultValue=false)]
        public System.DateTimeOffset? LeaseUntil { get; set; }
    
        /// <summary>
        /// Uppl&#229;tare
        /// </summary>
        [DataMember(Name="landlord",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate.Landlord Landlord { get; set; }
    
    }
}