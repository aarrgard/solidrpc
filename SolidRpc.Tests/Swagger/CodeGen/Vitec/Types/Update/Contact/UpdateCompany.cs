using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Models.Api;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CustomField.Models;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Update.Contact {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class UpdateCompany {
        /// <summary>
        /// F&#246;retagsnamn
        /// </summary>
        [DataMember(Name="companyName",EmitDefaultValue=false)]
        public string CompanyName { get; set; }
    
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
        /// Telefon v&#228;xel
        /// </summary>
        [DataMember(Name="switchPhone",EmitDefaultValue=false)]
        public string SwitchPhone { get; set; }
    
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
        /// Anv&#228;ndarid som kontakten ska kopplas till
        /// </summary>
        [DataMember(Name="userId",EmitDefaultValue=false)]
        public string UserId { get; set; }
    
        /// <summary>
        /// Telefon annat
        /// </summary>
        [DataMember(Name="otherPhone",EmitDefaultValue=false)]
        public string OtherPhone { get; set; }
    
        /// <summary>
        /// &#214;nskar reklamutskick
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
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Task.Models.Task Task { get; set; }
    
        /// <summary>
        /// Egendefinerat f&#228;lt
        /// </summary>
        [DataMember(Name="customField",EmitDefaultValue=false)]
        public FieldValueCriteria CustomField { get; set; }
    
    }
}