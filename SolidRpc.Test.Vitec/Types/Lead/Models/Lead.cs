using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Lead.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Lead {
        /// <summary>
        /// Id p� leads kontakten
        /// </summary>
        [DataMember(Name="contactId",EmitDefaultValue=false)]
        public string ContactId { get; set; }
    
        /// <summary>
        /// Id p� den firma, anv�ndargruppe eller anv�ndare som ska motta leadet
        /// </summary>
        [DataMember(Name="assignedToId",EmitDefaultValue=false)]
        public string AssignedToId { get; set; }
    
        /// <summary>
        /// Meddelande
        /// </summary>
        [DataMember(Name="message",EmitDefaultValue=false)]
        public string Message { get; set; }
    
        /// <summary>
        /// Intagsk�lla
        /// </summary>
        [DataMember(Name="assignmentSourceId",EmitDefaultValue=false)]
        public string AssignmentSourceId { get; set; }
    
        /// <summary>
        /// Id p� leadsk�llan
        /// </summary>
        [DataMember(Name="leadSourceId",EmitDefaultValue=false)]
        public string LeadSourceId { get; set; }
    
        /// <summary>
        /// Id p� anv�ndarens firma (om anv�ndare anges som mottagare av leads)
        /// </summary>
        [DataMember(Name="officeId",EmitDefaultValue=false)]
        public string OfficeId { get; set; }
    
    }
}