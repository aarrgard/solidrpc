using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class InvoiceRecipient {
        /// <summary>
        /// Typ av kontakt, f&#246;retag eller person
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
        /// <summary>
        /// Namn p&#229; f&#246;retag eller person som &#228;r mottagare till faktura
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Epost
        /// </summary>
        [DataMember(Name="email",EmitDefaultValue=false)]
        public string Email { get; set; }
    
        /// <summary>
        /// Organisationsnummer eller personnummer
        /// </summary>
        [DataMember(Name="identificationNumber",EmitDefaultValue=false)]
        public string IdentificationNumber { get; set; }
    
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
    
    }
}