using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Condominium {
        /// <summary>
        /// Fastighetsbeteckning
        /// </summary>
        [DataMember(Name="propertyDesignation",EmitDefaultValue=false)]
        public string PropertyDesignation { get; set; }
    
        /// <summary>
        /// Taxeringsv&#228;rden
        /// </summary>
        [DataMember(Name="taxation",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.Taxation Taxation { get; set; }
    
        /// <summary>
        /// Marknadsf&#246;ring
        /// </summary>
        [DataMember(Name="marketing",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.CondominiumMarketing Marketing { get; set; }
    
        /// <summary>
        /// Utgifter
        /// </summary>
        [DataMember(Name="expenses",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.CondominiumExpenses Expenses { get; set; }
    
        /// <summary>
        /// Projektid om l&#228;genheten ing&#229;r i ett projekt
        /// </summary>
        [DataMember(Name="projectId",EmitDefaultValue=false)]
        public string ProjectId { get; set; }
    
        /// <summary>
        /// Prisuppgift
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.Price Price { get; set; }
    
        /// <summary>
        /// Byggnad
        /// </summary>
        [DataMember(Name="building",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.AppartmentBuilding Building { get; set; }
    
        /// <summary>
        /// Energideklaration
        /// </summary>
        [DataMember(Name="energyDeclaration",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.EnergyDeclaration EnergyDeclaration { get; set; }
    
        /// <summary>
        /// Exteri&#246;r
        /// </summary>
        [DataMember(Name="exterior",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.AppartmentExterior Exterior { get; set; }
    
        /// <summary>
        /// Folkbokf&#246;ringsnummer
        /// </summary>
        [DataMember(Name="appartmentNumberInRegister",EmitDefaultValue=false)]
        public string AppartmentNumberInRegister { get; set; }
    
        /// <summary>
        /// Bostadens id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Status p&#229; f&#246;rs&#228;ljningen/uthyrningen.
        /// </summary>
        [DataMember(Name="status",EmitDefaultValue=false)]
        public string Status { get; set; }
    
        /// <summary>
        /// Kontorstillh&#246;righet
        /// </summary>
        [DataMember(Name="officeAffiliation",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.OfficeAffiliation OfficeAffiliation { get; set; }
    
        /// <summary>
        /// Ytterligare specialiseringsattribut
        /// </summary>
        [DataMember(Name="customAttributes",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.CustomAttributes CustomAttributes { get; set; }
    
        /// <summary>
        /// Id p&#229; huvudhandl&#228;ggare
        /// </summary>
        [DataMember(Name="primaryAgentId",EmitDefaultValue=false)]
        public string PrimaryAgentId { get; set; }
    
        /// <summary>
        /// Id p&#229; andrahandl&#228;ggare
        /// </summary>
        [DataMember(Name="secondaryAgentId",EmitDefaultValue=false)]
        public string SecondaryAgentId { get; set; }
    
        /// <summary>
        /// Adress och geografiska uppgifter
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.Address Address { get; set; }
    
        /// <summary>
        /// Texter
        /// </summary>
        [DataMember(Name="texts",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.Texts Texts { get; set; }
    
        /// <summary>
        /// N&#228;romr&#229;de
        /// </summary>
        [DataMember(Name="surroundings",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.Surroundings Surroundings { get; set; }
    
        /// <summary>
        /// Om budgivning p&#229;g&#229;r
        /// </summary>
        [DataMember(Name="acceptingBids",EmitDefaultValue=false)]
        public bool? AcceptingBids { get; set; }
    
        /// <summary>
        /// N&#228;r bostaden senast &#228;ndrades
        /// </summary>
        [DataMember(Name="changedAt",EmitDefaultValue=false)]
        public DateTimeOffset? ChangedAt { get; set; }
    
        /// <summary>
        /// Tilltr&#228;de vid
        /// </summary>
        [DataMember(Name="admissionAt",EmitDefaultValue=false)]
        public DateTimeOffset? AdmissionAt { get; set; }
    
        /// <summary>
        /// Url:er
        /// </summary>
        [DataMember(Name="urls",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.Urls Urls { get; set; }
    
        /// <summary>
        /// Filer
        /// </summary>
        [DataMember(Name="files",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Media.PublicAdvertisement.File> Files { get; set; }
    
        /// <summary>
        /// Visningar
        /// </summary>
        [DataMember(Name="viewings",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.Viewing> Viewings { get; set; }
    
        /// <summary>
        /// Bilder
        /// </summary>
        [DataMember(Name="images",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.Image> Images { get; set; }
    
    }
}