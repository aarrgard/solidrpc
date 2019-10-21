using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Date {
        /// <summary>
        /// M&#246;jligt tilltr&#228;desdatum
        /// </summary>
        [DataMember(Name="possibleAccessDate",EmitDefaultValue=false)]
        public string PossibleAccessDate { get; set; }
    
        /// <summary>
        /// Tilltr&#228;desdatum
        /// </summary>
        [DataMember(Name="accessDate",EmitDefaultValue=false)]
        public DateTimeOffset AccessDate { get; set; }
    
        /// <summary>
        /// Tilltr&#228;detid
        /// </summary>
        [DataMember(Name="accessTime",EmitDefaultValue=false)]
        public string AccessTime { get; set; }
    
        /// <summary>
        /// Kontraktsdag
        /// </summary>
        [DataMember(Name="agreementDate",EmitDefaultValue=false)]
        public DateTimeOffset AgreementDate { get; set; }
    
        /// <summary>
        /// Uppdragsdatum
        /// </summary>
        [DataMember(Name="assignmentDate",EmitDefaultValue=false)]
        public DateTimeOffset AssignmentDate { get; set; }
    
        /// <summary>
        /// Alla avtalsvillkor uppfyllda
        /// </summary>
        [DataMember(Name="allTermsMettDate",EmitDefaultValue=false)]
        public DateTimeOffset AllTermsMettDate { get; set; }
    
    }
}