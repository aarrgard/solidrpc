using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Models.Api;
using System;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.Crm.Contact {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CrmContact {
        /// <summary>
        /// Kontakt id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Kontakttyp (Person, f&#246;retag eller d&#246;dsbo)
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
        /// <summary>
        /// Kund-id
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// M&#228;klarens id
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
        /// Reklamutskick till&#229;ts
        /// </summary>
        [DataMember(Name="advertisingEnabled",EmitDefaultValue=false)]
        public bool? AdvertisingEnabled { get; set; }
    
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
        public DateTimeOffset? CreatedAt { get; set; }
    
        /// <summary>
        /// &#196;ndrad
        /// </summary>
        [DataMember(Name="changedAt",EmitDefaultValue=false)]
        public DateTimeOffset? ChangedAt { get; set; }
    
    }
}