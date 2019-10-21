using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EstateAgent {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Namn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// E-postadress
        /// </summary>
        [DataMember(Name="emailAddress",EmitDefaultValue=false)]
        public string EmailAddress { get; set; }
    
        /// <summary>
        /// N&#228;r m&#228;klaren senast &#228;ndrades
        /// </summary>
        [DataMember(Name="changedAt",EmitDefaultValue=false)]
        public DateTimeOffset ChangedAt { get; set; }
    
        /// <summary>
        /// Telefonnummer
        /// </summary>
        [DataMember(Name="telephone",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.TelephoneNumbers Telephone { get; set; }
    
        /// <summary>
        /// M&#228;klarens tillh&#246;righeter till kontor
        /// </summary>
        [DataMember(Name="officeAffiliations",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.EstateAgentOfficeAffiliation> OfficeAffiliations { get; set; }
    
        /// <summary>
        /// Titel
        /// </summary>
        [DataMember(Name="title",EmitDefaultValue=false)]
        public string Title { get; set; }
    
        /// <summary>
        /// Bild p&#229; m&#228;klaren
        /// </summary>
        [DataMember(Name="image",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models.Image Image { get; set; }
    
    }
}