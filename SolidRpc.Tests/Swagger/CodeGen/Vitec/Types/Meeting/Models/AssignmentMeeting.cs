using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Meeting.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AssignmentMeeting {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="startsAt",EmitDefaultValue=false)]
        public DateTimeOffset StartsAt { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="endsAt",EmitDefaultValue=false)]
        public DateTimeOffset EndsAt { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="location",EmitDefaultValue=false)]
        public string Location { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="note",EmitDefaultValue=false)]
        public string Note { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="contactId",EmitDefaultValue=false)]
        public string ContactId { get; set; }
    
    }
}