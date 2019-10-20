using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Lease {
        /// <summary>
        /// Arrende kr/�r
        /// </summary>
        [DataMember(Name="leaseFee",EmitDefaultValue=false)]
        public double LeaseFee { get; set; }
    
        /// <summary>
        /// Kommentar
        /// </summary>
        [DataMember(Name="leaseFeeComment",EmitDefaultValue=false)]
        public string LeaseFeeComment { get; set; }
    
        /// <summary>
        /// L�ptid
        /// </summary>
        [DataMember(Name="leaseUntil",EmitDefaultValue=false)]
        public DateTimeOffset LeaseUntil { get; set; }
    
        /// <summary>
        /// Uppl�tare
        /// </summary>
        [DataMember(Name="landlord",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Common.Estate.Landlord Landlord { get; set; }
    
    }
}