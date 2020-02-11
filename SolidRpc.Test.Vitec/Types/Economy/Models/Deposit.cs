using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Economy.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Deposit {
        /// <summary>
        /// Belopp
        /// </summary>
        [DataMember(Name="amount",EmitDefaultValue=false)]
        public double Amount { get; set; }
    
        /// <summary>
        /// Dag f&#246;r erl&#228;ggning
        /// </summary>
        [DataMember(Name="payDay",EmitDefaultValue=false)]
        public DateTimeOffset PayDay { get; set; }
    
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
    
        /// <summary>
        /// Kontraktsdag
        /// </summary>
        [DataMember(Name="contractDay",EmitDefaultValue=false)]
        public DateTimeOffset ContractDay { get; set; }
    
        /// <summary>
        /// Tilltr&#228;de
        /// </summary>
        [DataMember(Name="admissionDay",EmitDefaultValue=false)]
        public DateTimeOffset AdmissionDay { get; set; }
    
    }
}