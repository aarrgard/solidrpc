using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Viewing.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Participant {
        /// <summary>
        /// Deltog p&#229; visningen
        /// </summary>
        [DataMember(Name="hasParticipated",EmitDefaultValue=false)]
        public bool? HasParticipated { get; set; }
    
        /// <summary>
        /// Kontaktid
        /// </summary>
        [DataMember(Name="contactId",EmitDefaultValue=false)]
        public string ContactId { get; set; }
    
    }
}