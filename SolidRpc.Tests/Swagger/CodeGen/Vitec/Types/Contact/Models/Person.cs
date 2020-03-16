using System.CodeDom.Compiler;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Interest.Models;
using System.Runtime.Serialization;
using System;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CustomField.Models;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Models.Api;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Contact.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Person {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="lookingForAccommodation",EmitDefaultValue=false)]
        public IEnumerable<LookingForAccommodation> LookingForAccommodation { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="presentAccommodation",EmitDefaultValue=false)]
        public PresentAccommodation PresentAccommodation { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="buyerOnObjects",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Contact.Models.BuyerOnObject> BuyerOnObjects { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="sellerOnObjects",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Contact.Models.SellerOnObject> SellerOnObjects { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="interestOnObjects",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Contact.Models.IntrestOnObject> InterestOnObjects { get; set; }
    
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
        /// Telefon bostad
        /// </summary>
        [DataMember(Name="telePhone",EmitDefaultValue=false)]
        public string TelePhone { get; set; }
    
        /// <summary>
        /// Telefon arbete
        /// </summary>
        [DataMember(Name="workPhone",EmitDefaultValue=false)]
        public string WorkPhone { get; set; }
    
        /// <summary>
        /// Mobiltelefon
        /// </summary>
        [DataMember(Name="cellPhone",EmitDefaultValue=false)]
        public string CellPhone { get; set; }
    
        /// <summary>
        /// GDPR informerad den
        /// </summary>
        [DataMember(Name="gdprApprovalDate",EmitDefaultValue=false)]
        public System.DateTimeOffset? GdprApprovalDate { get; set; }
    
        /// <summary>
        /// Informerad via
        /// </summary>
        [DataMember(Name="obtainThrough",EmitDefaultValue=false)]
        public string ObtainThrough { get; set; }
    
        /// <summary>
        /// M&#228;klarens id
        /// </summary>
        [DataMember(Name="brokerId",EmitDefaultValue=false)]
        public string BrokerId { get; set; }
    
        /// <summary>
        /// Kontakt id
        /// </summary>
        [DataMember(Name="contactId",EmitDefaultValue=false)]
        public string ContactId { get; set; }
    
        /// <summary>
        /// Kundid
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Egendefinierade f&#228;lt
        /// </summary>
        [DataMember(Name="customFields",EmitDefaultValue=false)]
        public IEnumerable<FieldValue> CustomFields { get; set; }
    
        /// <summary>
        /// Kontaktkategori id
        /// </summary>
        [DataMember(Name="categories",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Category.Models.Category> Categories { get; set; }
    
        /// <summary>
        /// Adress
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public Address Address { get; set; }
    
        /// <summary>
        /// Email
        /// </summary>
        [DataMember(Name="email",EmitDefaultValue=false)]
        public Email Email { get; set; }
    
        /// <summary>
        /// Telefon annat
        /// </summary>
        [DataMember(Name="otherPhone",EmitDefaultValue=false)]
        public string OtherPhone { get; set; }
    
        /// <summary>
        /// &#214;nskar reklamutskick
        /// </summary>
        [DataMember(Name="wishAdvertising",EmitDefaultValue=false)]
        public bool? WishAdvertising { get; set; }
    
        /// <summary>
        /// Anteckning
        /// </summary>
        [DataMember(Name="note",EmitDefaultValue=false)]
        public string Note { get; set; }
    
        /// <summary>
        /// Koordinater
        /// </summary>
        [DataMember(Name="coordinate",EmitDefaultValue=false)]
        public Coordinate Coordinate { get; set; }
    
        /// <summary>
        /// Skapad
        /// </summary>
        [DataMember(Name="createdAt",EmitDefaultValue=false)]
        public System.DateTimeOffset? CreatedAt { get; set; }
    
        /// <summary>
        /// &#196;ndrad
        /// </summary>
        [DataMember(Name="changedAt",EmitDefaultValue=false)]
        public System.DateTimeOffset? ChangedAt { get; set; }
    
    }
}