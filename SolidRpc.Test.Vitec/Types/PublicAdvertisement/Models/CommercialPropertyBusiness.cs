using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Models.Api;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CommercialPropertyBusiness {
        /// <summary>
        /// Bransch
        /// </summary>
        [DataMember(Name="lineOfBusiness",EmitDefaultValue=false)]
        public string LineOfBusiness { get; set; }
    
        /// <summary>
        /// Oms�ttning
        /// </summary>
        [DataMember(Name="revenue",EmitDefaultValue=false)]
        public MoneyValue Revenue { get; set; }
    
        /// <summary>
        /// Resultat
        /// </summary>
        [DataMember(Name="profitable",EmitDefaultValue=false)]
        public string Profitable { get; set; }
    
        /// <summary>
        /// Verksamhet
        /// </summary>
        [DataMember(Name="activity",EmitDefaultValue=false)]
        public string Activity { get; set; }
    
        /// <summary>
        /// Antal anst�llda
        /// </summary>
        [DataMember(Name="numberOfEmployees",EmitDefaultValue=false)]
        public int NumberOfEmployees { get; set; }
    
        /// <summary>
        /// Utrustning som medf�ljer
        /// </summary>
        [DataMember(Name="equipment",EmitDefaultValue=false)]
        public string Equipment { get; set; }
    
        /// <summary>
        /// Start�r
        /// </summary>
        [DataMember(Name="establishedYear",EmitDefaultValue=false)]
        public int EstablishedYear { get; set; }
    
    }
}