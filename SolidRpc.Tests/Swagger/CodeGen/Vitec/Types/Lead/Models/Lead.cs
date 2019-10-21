using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Lead.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Lead {
        /// <summary>
        /// Id p&#229; leads kontakten
        /// </summary>
        [DataMember(Name="contactId",EmitDefaultValue=false)]
        public string ContactId { get; set; }
    
        /// <summary>
        /// Id p&#229; den firma, anv&#228;ndargruppe eller anv&#228;ndare som ska motta leadet
        /// </summary>
        [DataMember(Name="assignedToId",EmitDefaultValue=false)]
        public string AssignedToId { get; set; }
    
        /// <summary>
        /// Meddelande
        /// </summary>
        [DataMember(Name="message",EmitDefaultValue=false)]
        public string Message { get; set; }
    
        /// <summary>
        /// Intagsk&#228;lla
        /// </summary>
        [DataMember(Name="assignmentSourceId",EmitDefaultValue=false)]
        public string AssignmentSourceId { get; set; }
    
        /// <summary>
        /// Id p&#229; leadsk&#228;llan
        /// </summary>
        [DataMember(Name="leadSourceId",EmitDefaultValue=false)]
        public string LeadSourceId { get; set; }
    
        /// <summary>
        /// Id p&#229; anv&#228;ndarens firma (om anv&#228;ndare anges som mottagare av leads)
        /// </summary>
        [DataMember(Name="officeId",EmitDefaultValue=false)]
        public string OfficeId { get; set; }
    
    }
}