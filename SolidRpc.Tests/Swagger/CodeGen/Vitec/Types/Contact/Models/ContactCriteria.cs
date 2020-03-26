using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CustomField.Models;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Contact.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ContactCriteria {
        /// <summary>
        /// Om kontakt id ska tillh&#246;ra en specifikt kontor
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Kontaktid
        /// </summary>
        [DataMember(Name="contactId",EmitDefaultValue=false)]
        public IEnumerable<string> ContactId { get; set; }
    
        /// <summary>
        /// Urval p&#229; anv&#228;ndarid
        /// </summary>
        [DataMember(Name="userId",EmitDefaultValue=false)]
        public string UserId { get; set; }
    
        /// <summary>
        /// Urval p&#229; kategorier
        /// </summary>
        [DataMember(Name="categories",EmitDefaultValue=false)]
        public IEnumerable<string> Categories { get; set; }
    
        /// <summary>
        /// Urval p&#229; epostadress
        /// </summary>
        [DataMember(Name="emailAddresses",EmitDefaultValue=false)]
        public IEnumerable<string> EmailAddresses { get; set; }
    
        /// <summary>
        /// Kontraktdatum fr&#229;n
        /// </summary>
        [DataMember(Name="contractDateFrom",EmitDefaultValue=false)]
        public DateTimeOffset? ContractDateFrom { get; set; }
    
        /// <summary>
        /// Kontraktdatum till
        /// </summary>
        [DataMember(Name="contractDateTo",EmitDefaultValue=false)]
        public DateTimeOffset? ContractDateTo { get; set; }
    
        /// <summary>
        /// Visiningsdatum fr&#229;n
        /// </summary>
        [DataMember(Name="viewingDateFrom",EmitDefaultValue=false)]
        public DateTimeOffset? ViewingDateFrom { get; set; }
    
        /// <summary>
        /// Visiningsdatum till
        /// </summary>
        [DataMember(Name="viewingDateTo",EmitDefaultValue=false)]
        public DateTimeOffset? ViewingDateTo { get; set; }
    
        /// <summary>
        /// Budgivningsdatum fr&#229;n
        /// </summary>
        [DataMember(Name="biddingDateFrom",EmitDefaultValue=false)]
        public DateTimeOffset? BiddingDateFrom { get; set; }
    
        /// <summary>
        /// Budgivningsdatum till
        /// </summary>
        [DataMember(Name="biddingDateTo",EmitDefaultValue=false)]
        public DateTimeOffset? BiddingDateTo { get; set; }
    
        /// <summary>
        /// Skapatdatum fr&#229;n
        /// </summary>
        [DataMember(Name="createdDateFrom",EmitDefaultValue=false)]
        public DateTimeOffset? CreatedDateFrom { get; set; }
    
        /// <summary>
        /// Skapatdatum till
        /// </summary>
        [DataMember(Name="createdDateTo",EmitDefaultValue=false)]
        public DateTimeOffset? CreatedDateTo { get; set; }
    
        /// <summary>
        /// &#196;ndringsdatum fr&#229;n
        /// </summary>
        [DataMember(Name="changedDateFrom",EmitDefaultValue=false)]
        public DateTimeOffset? ChangedDateFrom { get; set; }
    
        /// <summary>
        /// &#196;ndringsdatum till
        /// </summary>
        [DataMember(Name="changedDateTo",EmitDefaultValue=false)]
        public DateTimeOffset? ChangedDateTo { get; set; }
    
        /// <summary>
        /// Fr&#229;n datum kopplad som s&#228;ljare p&#229; objekt
        /// </summary>
        [DataMember(Name="sellerRelationDateFrom",EmitDefaultValue=false)]
        public DateTimeOffset? SellerRelationDateFrom { get; set; }
    
        /// <summary>
        /// Till datum kopplad som s&#228;ljare p&#229; objekt
        /// </summary>
        [DataMember(Name="sellerRelationDateTo",EmitDefaultValue=false)]
        public DateTimeOffset? SellerRelationDateTo { get; set; }
    
        /// <summary>
        /// Fr&#229;n datum kopplad som k&#246;pare p&#229; objekt
        /// </summary>
        [DataMember(Name="buyerRelationDateFrom",EmitDefaultValue=false)]
        public DateTimeOffset? BuyerRelationDateFrom { get; set; }
    
        /// <summary>
        /// Till datum kopplad som k&#246;pare p&#229; objekt
        /// </summary>
        [DataMember(Name="buyerRelationDateTo",EmitDefaultValue=false)]
        public DateTimeOffset? BuyerRelationDateTo { get; set; }
    
        /// <summary>
        /// Egendefinerat f&#228;lt
        /// </summary>
        [DataMember(Name="customField",EmitDefaultValue=false)]
        public FieldValueCriteria CustomField { get; set; }
    
        /// <summary>
        /// Personnummer
        /// </summary>
        [DataMember(Name="socialSecurityNumber",EmitDefaultValue=false)]
        public string SocialSecurityNumber { get; set; }
    
    }
}