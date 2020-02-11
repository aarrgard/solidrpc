using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Models.Api;
namespace SolidRpc.Test.Vitec.Types.PartnerService.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PersonLead {
        /// <summary>
        /// F&#246;rnamn
        /// </summary>
        [DataMember(Name="firstName",EmitDefaultValue=false)]
        public string FirstName { get; set; }
    
        /// <summary>
        /// Efternamn
        /// </summary>
        [DataMember(Name="lastName",EmitDefaultValue=false)]
        public string LastName { get; set; }
    
        /// <summary>
        /// Personnummer
        /// </summary>
        [DataMember(Name="socialSecurityNumber",EmitDefaultValue=false)]
        public string SocialSecurityNumber { get; set; }
    
        /// <summary>
        /// Telefonnummer
        /// </summary>
        [DataMember(Name="telephone",EmitDefaultValue=false)]
        public PersonTelephoneNumbers Telephone { get; set; }
    
        /// <summary>
        /// Meddelande
        /// </summary>
        [DataMember(Name="message",EmitDefaultValue=false)]
        public string Message { get; set; }
    
        /// <summary>
        /// Typ av lead
        /// </summary>
        [DataMember(Name="typeId",EmitDefaultValue=false)]
        public string TypeId { get; set; }
    
        /// <summary>
        /// Intagsk&#228;lla
        /// </summary>
        [DataMember(Name="assignmentSourceId",EmitDefaultValue=false)]
        public string AssignmentSourceId { get; set; }
    
        /// <summary>
        /// Adress
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PartnerService.Models.ContactAddress Address { get; set; }
    
        /// <summary>
        /// Email
        /// </summary>
        [DataMember(Name="email",EmitDefaultValue=false)]
        public Email Email { get; set; }
    
        /// <summary>
        /// Kontor, mottagare av tipset
        /// </summary>
        [DataMember(Name="office",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PartnerService.Models.ContactOffice Office { get; set; }
    
        /// <summary>
        /// Anv&#228;ndare, mottagare av tipset
        /// </summary>
        [DataMember(Name="agent",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PartnerService.Models.ContactAgent Agent { get; set; }
    
        /// <summary>
        /// Avs&#228;ndare av tipset
        /// </summary>
        [DataMember(Name="sender",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PartnerService.Models.ContactLeadSender Sender { get; set; }
    
    }
}