using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Models.Api;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Office.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Office {
        /// <summary>
        /// Kundid
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Officeid
        /// </summary>
        [DataMember(Name="officeId",EmitDefaultValue=false)]
        public string OfficeId { get; set; }
    
        /// <summary>
        /// F�retagsnamn
        /// </summary>
        [DataMember(Name="customerName",EmitDefaultValue=false)]
        public string CustomerName { get; set; }
    
        /// <summary>
        /// Juridiskt f�retagsnamn
        /// </summary>
        [DataMember(Name="legalCustomerName",EmitDefaultValue=false)]
        public string LegalCustomerName { get; set; }
    
        /// <summary>
        /// Gatuadress
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public string Address { get; set; }
    
        /// <summary>
        /// Postadress
        /// </summary>
        [DataMember(Name="postalAddress",EmitDefaultValue=false)]
        public string PostalAddress { get; set; }
    
        /// <summary>
        /// Postnummer
        /// </summary>
        [DataMember(Name="zipCode",EmitDefaultValue=false)]
        public string ZipCode { get; set; }
    
        /// <summary>
        /// Ort
        /// </summary>
        [DataMember(Name="city",EmitDefaultValue=false)]
        public string City { get; set; }
    
        /// <summary>
        /// Telefon
        /// </summary>
        [DataMember(Name="telephone",EmitDefaultValue=false)]
        public string Telephone { get; set; }
    
        /// <summary>
        /// F�retagets s�tte
        /// </summary>
        [DataMember(Name="companyPlace",EmitDefaultValue=false)]
        public string CompanyPlace { get; set; }
    
        /// <summary>
        /// E-postadress
        /// </summary>
        [DataMember(Name="emailAddress",EmitDefaultValue=false)]
        public string EmailAddress { get; set; }
    
        /// <summary>
        /// Hemsida
        /// </summary>
        [DataMember(Name="homePage",EmitDefaultValue=false)]
        public string HomePage { get; set; }
    
        /// <summary>
        /// Beskrivning Hemsida
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public string Description { get; set; }
    
        /// <summary>
        /// Organisationsnummer
        /// </summary>
        [DataMember(Name="corporateNumber",EmitDefaultValue=false)]
        public string CorporateNumber { get; set; }
    
        /// <summary>
        /// Bankgiro
        /// </summary>
        [DataMember(Name="bankgiro",EmitDefaultValue=false)]
        public string Bankgiro { get; set; }
    
        /// <summary>
        /// Plusgiro
        /// </summary>
        [DataMember(Name="postalgiro",EmitDefaultValue=false)]
        public string Postalgiro { get; set; }
    
        /// <summary>
        /// Momsregistreringsnummer
        /// </summary>
        [DataMember(Name="vat",EmitDefaultValue=false)]
        public string Vat { get; set; }
    
        /// <summary>
        /// F-skattebevis nummer
        /// </summary>
        [DataMember(Name="taxCertificate",EmitDefaultValue=false)]
        public string TaxCertificate { get; set; }
    
        /// <summary>
        /// Kordinater
        /// </summary>
        [DataMember(Name="coordinate",EmitDefaultValue=false)]
        public Coordinate Coordinate { get; set; }
    
        /// <summary>
        /// Region
        /// </summary>
        [DataMember(Name="region",EmitDefaultValue=false)]
        public int Region { get; set; }
    
        /// <summary>
        /// Internt kontorsnummer
        /// </summary>
        [DataMember(Name="officeNumber",EmitDefaultValue=false)]
        public int OfficeNumber { get; set; }
    
        /// <summary>
        /// Internt kontorsnamn
        /// </summary>
        [DataMember(Name="officeName",EmitDefaultValue=false)]
        public string OfficeName { get; set; }
    
        /// <summary>
        /// Kedjetillh�righet
        /// </summary>
        [DataMember(Name="chain",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Office.Models.Chain Chain { get; set; }
    
        /// <summary>
        /// �ndringsdatum
        /// </summary>
        [DataMember(Name="dateChanged",EmitDefaultValue=false)]
        public DateTimeOffset DateChanged { get; set; }
    
        /// <summary>
        /// Prim�r tips/lead mottagare
        /// </summary>
        [DataMember(Name="primaryLeadReceiverId",EmitDefaultValue=false)]
        public string PrimaryLeadReceiverId { get; set; }
    
    }
}