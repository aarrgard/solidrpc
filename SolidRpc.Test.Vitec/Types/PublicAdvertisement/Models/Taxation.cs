using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Taxation {
        /// <summary>
        /// Taxeringsv&#228;rde byggnad (SEK)
        /// </summary>
        [DataMember(Name="buildingValue",EmitDefaultValue=false)]
        public double BuildingValue { get; set; }
    
        /// <summary>
        /// Typkod
        /// </summary>
        [DataMember(Name="code",EmitDefaultValue=false)]
        public string Code { get; set; }
    
        /// <summary>
        /// Taxeringsv&#228;rde totalt (SEK)
        /// </summary>
        [DataMember(Name="totalValue",EmitDefaultValue=false)]
        public double TotalValue { get; set; }
    
        /// <summary>
        /// Taxerings&#229;r
        /// </summary>
        [DataMember(Name="year",EmitDefaultValue=false)]
        public int Year { get; set; }
    
    }
}