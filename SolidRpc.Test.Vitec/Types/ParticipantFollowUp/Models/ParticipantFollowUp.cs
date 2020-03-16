using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.ParticipantFollowUp.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ParticipantFollowUp {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="contactId",EmitDefaultValue=false)]
        public string ContactId { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="viewingId",EmitDefaultValue=false)]
        public string ViewingId { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="participated",EmitDefaultValue=false)]
        public bool? Participated { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="contacted",EmitDefaultValue=false)]
        public string Contacted { get; set; }
    
    }
}