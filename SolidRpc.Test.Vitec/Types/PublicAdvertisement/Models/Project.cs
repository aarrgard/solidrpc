using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Project {
        /// <summary>
        /// Projektets id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Projektets namn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Marknadsf&#246;ring
        /// </summary>
        [DataMember(Name="marketing",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.ProjectMarketing Marketing { get; set; }
    
        /// <summary>
        /// Kontorstillh&#246;righet
        /// </summary>
        [DataMember(Name="officeAffiliation",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.OfficeAffiliation OfficeAffiliation { get; set; }
    
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
        /// N&#228;r bostaden senast &#228;ndrades
        /// </summary>
        [DataMember(Name="changedAt",EmitDefaultValue=false)]
        public DateTimeOffset ChangedAt { get; set; }
    
        /// <summary>
        /// Url:er
        /// </summary>
        [DataMember(Name="urls",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.Urls Urls { get; set; }
    
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
    
        /// <summary>
        /// Villor som ing&#229;r i projektet
        /// </summary>
        [DataMember(Name="houses",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.ProjectEstateRelation> Houses { get; set; }
    
        /// <summary>
        /// Tomter som ing&#229;r i projektet
        /// </summary>
        [DataMember(Name="plots",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.ProjectEstateRelation> Plots { get; set; }
    
        /// <summary>
        /// Bostadsr&#228;tter som ing&#229;r i projektet
        /// </summary>
        [DataMember(Name="housingCooperatives",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.ProjectEstateRelation> HousingCooperatives { get; set; }
    
        /// <summary>
        /// &#196;garl&#228;genheter som ing&#229;r i projektet
        /// </summary>
        [DataMember(Name="condominiums",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.ProjectEstateRelation> Condominiums { get; set; }
    
        /// <summary>
        /// Lokaler som ing&#229;r i projektet
        /// </summary>
        [DataMember(Name="premises",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.ProjectEstateRelation> Premises { get; set; }
    
        /// <summary>
        /// Utl&#228;ndska bost&#228;der som ing&#229;r i projektet
        /// </summary>
        [DataMember(Name="foreignProperties",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.ProjectEstateRelation> ForeignProperties { get; set; }
    
        /// <summary>
        /// Fritidshus som ing&#229;r i projektet
        /// </summary>
        [DataMember(Name="cottages",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models.ProjectEstateRelation> Cottages { get; set; }
    
    }
}