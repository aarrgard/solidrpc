using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Viewing.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class TimeSlot {
        /// <summary>
        /// Id p&#229; visningstillf&#228;llet
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Vilken tid b&#246;rjar visningstillf&#228;llet
        /// </summary>
        [DataMember(Name="startsAt",EmitDefaultValue=false)]
        public DateTimeOffset? StartsAt { get; set; }
    
        /// <summary>
        /// Vilken tid slutar visningstillf&#228;llet
        /// </summary>
        [DataMember(Name="endsAt",EmitDefaultValue=false)]
        public DateTimeOffset? EndsAt { get; set; }
    
        /// <summary>
        /// Deltagare p&#229; visningen
        /// </summary>
        [DataMember(Name="participants",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Viewing.Models.Participant> Participants { get; set; }
    
    }
}