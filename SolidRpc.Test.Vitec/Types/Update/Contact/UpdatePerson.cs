using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Models.Api;
using SolidRpc.Test.Vitec.Types.CustomField.Models;
namespace SolidRpc.Test.Vitec.Types.Update.Contact {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class UpdatePerson {
        /// <summary>
        /// F�rnamn
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
        /// PUL-Godk�nnande
        /// </summary>
        [DataMember(Name="approval",EmitDefaultValue=false)]
        public bool Approval { get; set; }
    
        /// <summary>
        /// PUL-Godk�nnande datum
        /// </summary>
        [DataMember(Name="approvalDate",EmitDefaultValue=false)]
        public DateTimeOffset ApprovalDate { get; set; }
    
        /// <summary>
        /// GDPR informerad den
        /// </summary>
        [DataMember(Name="gdprApprovalDate",EmitDefaultValue=false)]
        public DateTimeOffset GdprApprovalDate { get; set; }
    
        /// <summary>
        /// Informerad via
        /// </summary>
        [DataMember(Name="obtainThrough",EmitDefaultValue=false)]
        public string ObtainThrough { get; set; }
    
        /// <summary>
        /// Kontakt id
        /// </summary>
        [DataMember(Name="contactId",EmitDefaultValue=false)]
        public string ContactId { get; set; }
    
        /// <summary>
        /// Kund Id
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Kontaktkategori id
        /// </summary>
        [DataMember(Name="categoryIds",EmitDefaultValue=false)]
        public IEnumerable<string> CategoryIds { get; set; }
    
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
        /// Anv�ndarid som kontakten ska kopplas till
        /// </summary>
        [DataMember(Name="userId",EmitDefaultValue=false)]
        public string UserId { get; set; }
    
        /// <summary>
        /// Telefon annat
        /// </summary>
        [DataMember(Name="otherPhone",EmitDefaultValue=false)]
        public string OtherPhone { get; set; }
    
        /// <summary>
        /// �nskar reklamutskick
        /// </summary>
        [DataMember(Name="wishAdvertising",EmitDefaultValue=false)]
        public bool WishAdvertising { get; set; }
    
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
        /// Typad uppgift
        /// </summary>
        [DataMember(Name="task",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Task.Models.Task Task { get; set; }
    
        /// <summary>
        /// Egendefinerat f�lt
        /// </summary>
        [DataMember(Name="customField",EmitDefaultValue=false)]
        public FieldValueCriteria CustomField { get; set; }
    
    }
}