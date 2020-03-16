using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.InterestApplication.Models;
using SolidRpc.Test.Vitec.Types.Models.Api;
using System;
namespace SolidRpc.Test.Vitec.Types.Advertising.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AdvertisingInterestApplication {
        /// <summary>
        /// Leadsk&#228;lla - Det lead som skapas i samband med intresseanm&#228;lan kommer att kopplas till aktuell leadsk&#228;lla. Om f&#228;ltet inte anges, kommer leadet att kopplas till en f&#246;rvald leadsk&#228;lla.
        /// </summary>
        [DataMember(Name="leadSourceId",EmitDefaultValue=false)]
        public string LeadSourceId { get; set; }
    
        /// <summary>
        /// Intagsk&#228;lla f&#246;r den bostad som personen eventuellt l&#228;mnar. Skall bara anges om angiven leadsk&#228;lla anv&#228;nds f&#246;r att koppla leadet till en bostad som kontaktpersonen eventuellt skall s&#228;lja.
        /// </summary>
        [DataMember(Name="assignmentSourceId",EmitDefaultValue=false)]
        public string AssignmentSourceId { get; set; }
    
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
        /// Adress
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public ContactAddress Address { get; set; }
    
        /// <summary>
        /// Telefonnummer
        /// </summary>
        [DataMember(Name="telephone",EmitDefaultValue=false)]
        public PersonTelephoneNumbers Telephone { get; set; }
    
        /// <summary>
        /// Email
        /// </summary>
        [DataMember(Name="email",EmitDefaultValue=false)]
        public Email Email { get; set; }
    
        /// <summary>
        /// GDPR informerad den
        /// </summary>
        [DataMember(Name="gdprApprovalDate",EmitDefaultValue=false)]
        public DateTimeOffset? GdprApprovalDate { get; set; }
    
        /// <summary>
        /// Uppgifter om nuvarande boende
        /// </summary>
        [DataMember(Name="presentAccommodation",EmitDefaultValue=false)]
        public ContactPresentAccommodation PresentAccommodation { get; set; }
    
        /// <summary>
        /// Meddelande fr&#229;n kontaktpersonen
        /// </summary>
        [DataMember(Name="contactMessage",EmitDefaultValue=false)]
        public string ContactMessage { get; set; }
    
    }
}