using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Models.Api;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Price {
        /// <summary>
        /// Utg�ngspris (SEK)
        /// </summary>
        [DataMember(Name="swedishCurrency",EmitDefaultValue=false)]
        public double SwedishCurrency { get; set; }
    
        /// <summary>
        /// Pris i annan valuta
        /// </summary>
        [DataMember(Name="foreignCurrency",EmitDefaultValue=false)]
        public MoneyValue ForeignCurrency { get; set; }
    
        /// <summary>
        /// Text
        /// </summary>
        [DataMember(Name="text",EmitDefaultValue=false)]
        public string Text { get; set; }
    
    }
}