using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PublicAdvertisingEstateListCriteria {
        /// <summary>
        /// Inkludera villor
        /// </summary>
        [DataMember(Name="includeHouses",EmitDefaultValue=false)]
        public bool IncludeHouses { get; set; }
    
        /// <summary>
        /// Inkludera l&#228;genheter (bostadsr&#228;tter)
        /// </summary>
        [DataMember(Name="includeHousingCooperatives",EmitDefaultValue=false)]
        public bool IncludeHousingCooperatives { get; set; }
    
        /// <summary>
        /// Inkludera fritidshus
        /// </summary>
        [DataMember(Name="includeCottages",EmitDefaultValue=false)]
        public bool IncludeCottages { get; set; }
    
        /// <summary>
        /// Inkludera tomter
        /// </summary>
        [DataMember(Name="includePlots",EmitDefaultValue=false)]
        public bool IncludePlots { get; set; }
    
        /// <summary>
        /// Inkludera lantbruk
        /// </summary>
        [DataMember(Name="includeFarms",EmitDefaultValue=false)]
        public bool IncludeFarms { get; set; }
    
        /// <summary>
        /// Inkludera kommersiella objekt
        /// </summary>
        [DataMember(Name="includeCommercialProperties",EmitDefaultValue=false)]
        public bool IncludeCommercialProperties { get; set; }
    
        /// <summary>
        /// Inkludera l&#228;genheter (&#228;gander&#228;tter)
        /// </summary>
        [DataMember(Name="includeCondominiums",EmitDefaultValue=false)]
        public bool IncludeCondominiums { get; set; }
    
        /// <summary>
        /// Inkludera utlandsobjekt
        /// </summary>
        [DataMember(Name="includeForeignProperties",EmitDefaultValue=false)]
        public bool IncludeForeignProperties { get; set; }
    
        /// <summary>
        /// Inkludera lokaler
        /// </summary>
        [DataMember(Name="includePremises",EmitDefaultValue=false)]
        public bool IncludePremises { get; set; }
    
        /// <summary>
        /// Inkludera projekt
        /// </summary>
        [DataMember(Name="includeProjects",EmitDefaultValue=false)]
        public bool IncludeProjects { get; set; }
    
        /// <summary>
        /// Inkludera bost&#228;der med status till salu
        /// </summary>
        [DataMember(Name="includeForSale",EmitDefaultValue=false)]
        public bool IncludeForSale { get; set; }
    
        /// <summary>
        /// Inkludera bost&#228;der med status kommande
        /// </summary>
        [DataMember(Name="includeFutureSale",EmitDefaultValue=false)]
        public bool IncludeFutureSale { get; set; }
    
        /// <summary>
        /// Inkludera bost&#228;der med status snart till salu
        /// </summary>
        [DataMember(Name="includeSoonForSale",EmitDefaultValue=false)]
        public bool IncludeSoonForSale { get; set; }
    
        /// <summary>
        /// Urval p&#229; huvudhandl&#228;ggare
        /// </summary>
        [DataMember(Name="primaryAgentId",EmitDefaultValue=false)]
        public string PrimaryAgentId { get; set; }
    
        /// <summary>
        /// Urval p&#229; bostadsid
        /// </summary>
        [DataMember(Name="estateId",EmitDefaultValue=false)]
        public string EstateId { get; set; }
    
    }
}