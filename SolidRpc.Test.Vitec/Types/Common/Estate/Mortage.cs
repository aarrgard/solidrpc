using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Mortage {
        /// <summary>
        /// Aktnummer
        /// </summary>
        [DataMember(Name="actNumber",EmitDefaultValue=false)]
        public string ActNumber { get; set; }
    
        /// <summary>
        /// Datum
        /// </summary>
        [DataMember(Name="date",EmitDefaultValue=false)]
        public DateTimeOffset Date { get; set; }
    
        /// <summary>
        /// Belopp
        /// </summary>
        [DataMember(Name="amount",EmitDefaultValue=false)]
        public double Amount { get; set; }
    
        /// <summary>
        /// �r ett skrifrligt pantbrev
        /// </summary>
        [DataMember(Name="writtenMortgages",EmitDefaultValue=false)]
        public bool WrittenMortgages { get; set; }
    
        /// <summary>
        /// Anteckning
        /// </summary>
        [DataMember(Name="note",EmitDefaultValue=false)]
        public string Note { get; set; }
    
        /// <summary>
        /// Inoml�ge
        /// </summary>
        [DataMember(Name="within",EmitDefaultValue=false)]
        public double Within { get; set; }
    
    }
}