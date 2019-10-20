using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Models.Api;
using System.Collections.Generic;
using System;
namespace SolidRpc.Test.Vitec.Types.Crm.Contact {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CrmCompanyContact {
        /// <summary>
        /// Kontakttyp (Person, f�retag eller d�dsbo)
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
        /// <summary>
        /// F�retagsnamn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Organisationsnummer
        /// </summary>
        [DataMember(Name="corporateNumber",EmitDefaultValue=false)]
        public string CorporateNumber { get; set; }
    
        /// <summary>
        /// Hemsida
        /// </summary>
        [DataMember(Name="homePage",EmitDefaultValue=false)]
        public string HomePage { get; set; }
    
        /// <summary>
        /// Telefonnummer
        /// </summary>
        [DataMember(Name="telephone",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Crm.Contact.CompanyTelephoneNumbers Telephone { get; set; }
    
        /// <summary>
        /// Kontakt id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Kund-id
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// M�klarens id
        /// </summary>
        [DataMember(Name="agentId",EmitDefaultValue=false)]
        public string AgentId { get; set; }
    
        /// <summary>
        /// Adress
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Crm.Contact.CrmContactAddress Address { get; set; }
    
        /// <summary>
        /// Email
        /// </summary>
        [DataMember(Name="email",EmitDefaultValue=false)]
        public Email Email { get; set; }
    
        /// <summary>
        /// Reklamutskick till�ts
        /// </summary>
        [DataMember(Name="advertisingEnabled",EmitDefaultValue=false)]
        public bool AdvertisingEnabled { get; set; }
    
        /// <summary>
        /// Anteckning
        /// </summary>
        [DataMember(Name="note",EmitDefaultValue=false)]
        public string Note { get; set; }
    
        /// <summary>
        /// Kategorier
        /// </summary>
        [DataMember(Name="categories",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Category.Models.Category> Categories { get; set; }
    
        /// <summary>
        /// Skapad
        /// </summary>
        [DataMember(Name="createdAt",EmitDefaultValue=false)]
        public DateTimeOffset CreatedAt { get; set; }
    
        /// <summary>
        /// �ndrad
        /// </summary>
        [DataMember(Name="changedAt",EmitDefaultValue=false)]
        public DateTimeOffset ChangedAt { get; set; }
    
    }
}