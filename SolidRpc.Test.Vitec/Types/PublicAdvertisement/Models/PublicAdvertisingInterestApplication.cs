using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.InterestApplication.Models;
using SolidRpc.Test.Vitec.Types.Models.Api;
using System;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PublicAdvertisingInterestApplication {
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