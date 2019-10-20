using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
namespace SolidRpc.Test.Vitec.Types.User.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class User {
        /// <summary>
        /// Arkiverad
        /// </summary>
        [DataMember(Name="archived",EmitDefaultValue=false)]
        public bool Archived { get; set; }
    
        /// <summary>
        /// Publik
        /// </summary>
        [DataMember(Name="public",EmitDefaultValue=false)]
        public bool Public { get; set; }
    
        /// <summary>
        /// Anv�ndarid
        /// </summary>
        [DataMember(Name="userId",EmitDefaultValue=false)]
        public string UserId { get; set; }
    
        /// <summary>
        /// Kontorsid
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public IEnumerable<string> CustomerId { get; set; }
    
        /// <summary>
        /// Sorteringsnummer
        /// </summary>
        [DataMember(Name="orderNumber",EmitDefaultValue=false)]
        public int OrderNumber { get; set; }
    
        /// <summary>
        /// Namn
        /// </summary>
        [DataMember(Name="userName",EmitDefaultValue=false)]
        public string UserName { get; set; }
    
        /// <summary>
        /// Avdelning
        /// </summary>
        [DataMember(Name="department",EmitDefaultValue=false)]
        public string Department { get; set; }
    
        /// <summary>
        /// Kategori
        /// </summary>
        [DataMember(Name="category",EmitDefaultValue=false)]
        public string Category { get; set; }
    
        /// <summary>
        /// Titel
        /// </summary>
        [DataMember(Name="title",EmitDefaultValue=false)]
        public string Title { get; set; }
    
        /// <summary>
        /// Extra Titel
        /// </summary>
        [DataMember(Name="extraTitle",EmitDefaultValue=false)]
        public string ExtraTitle { get; set; }
    
        /// <summary>
        /// Till�t inloggning
        /// </summary>
        [DataMember(Name="allowLogOn",EmitDefaultValue=false)]
        public bool AllowLogOn { get; set; }
    
        /// <summary>
        /// Epostadress
        /// </summary>
        [DataMember(Name="emailAddress",EmitDefaultValue=false)]
        public string EmailAddress { get; set; }
    
        /// <summary>
        /// Telefonnummer
        /// </summary>
        [DataMember(Name="telePhone",EmitDefaultValue=false)]
        public string TelePhone { get; set; }
    
        /// <summary>
        /// Mobilnummer
        /// </summary>
        [DataMember(Name="cellPhone",EmitDefaultValue=false)]
        public string CellPhone { get; set; }
    
        /// <summary>
        /// Telefonnummer direkt
        /// </summary>
        [DataMember(Name="directPhonenumbers",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.User.Models.DirectPhoneNumber> DirectPhonenumbers { get; set; }
    
        /// <summary>
        /// Publikt telefonnummer
        /// </summary>
        [DataMember(Name="publicPhonenumber",EmitDefaultValue=false)]
        public string PublicPhonenumber { get; set; }
    
        /// <summary>
        /// Talade spr�k
        /// </summary>
        [DataMember(Name="spokenLanguages",EmitDefaultValue=false)]
        public IEnumerable<string> SpokenLanguages { get; set; }
    
        /// <summary>
        /// Banknamn
        /// </summary>
        [DataMember(Name="bank",EmitDefaultValue=false)]
        public string Bank { get; set; }
    
        /// <summary>
        /// Cleringnummer
        /// </summary>
        [DataMember(Name="cleringNumber",EmitDefaultValue=false)]
        public string CleringNumber { get; set; }
    
        /// <summary>
        /// Konto
        /// </summary>
        [DataMember(Name="account",EmitDefaultValue=false)]
        public string Account { get; set; }
    
        /// <summary>
        /// Ibannummer
        /// </summary>
        [DataMember(Name="iban",EmitDefaultValue=false)]
        public string Iban { get; set; }
    
        /// <summary>
        /// Swiftnummer
        /// </summary>
        [DataMember(Name="swift",EmitDefaultValue=false)]
        public string Swift { get; set; }
    
        /// <summary>
        /// Kommentar
        /// </summary>
        [DataMember(Name="comment",EmitDefaultValue=false)]
        public string Comment { get; set; }
    
        /// <summary>
        /// Bild
        /// </summary>
        [DataMember(Name="image",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Image.Models.Image Image { get; set; }
    
        /// <summary>
        /// F�retag d�r anv�ndaren �r publik
        /// </summary>
        [DataMember(Name="publishedOnOffice",EmitDefaultValue=false)]
        public IEnumerable<string> PublishedOnOffice { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="internalEmployeeNumber",EmitDefaultValue=false)]
        public int InternalEmployeeNumber { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="externalUserId",EmitDefaultValue=false)]
        public string ExternalUserId { get; set; }
    
        /// <summary>
        /// Kundid och sorteringnummer
        /// </summary>
        [DataMember(Name="customerIdsWithSortOrder",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.User.Models.CustomerIdWithOrderNumber> CustomerIdsWithSortOrder { get; set; }
    
        /// <summary>
        /// �ndringsdatum
        /// </summary>
        [DataMember(Name="dateChanged",EmitDefaultValue=false)]
        public DateTimeOffset DateChanged { get; set; }
    
    }
}