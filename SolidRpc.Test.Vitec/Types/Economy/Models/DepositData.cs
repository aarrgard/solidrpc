using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Economy.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class DepositData {
        /// <summary>
        /// Erlagd
        /// </summary>
        [DataMember(Name="paidAt",EmitDefaultValue=false)]
        public DateTimeOffset PaidAt { get; set; }
    
        /// <summary>
        /// Redovisad
        /// </summary>
        [DataMember(Name="reportedAt",EmitDefaultValue=false)]
        public DateTimeOffset ReportedAt { get; set; }
    
        /// <summary>
        /// Provision erh&#229;llen
        /// </summary>
        [DataMember(Name="commissionReceivedAt",EmitDefaultValue=false)]
        public DateTimeOffset CommissionReceivedAt { get; set; }
    
    }
}