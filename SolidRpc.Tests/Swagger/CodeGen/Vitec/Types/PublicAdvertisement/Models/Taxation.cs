using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Taxation {
        /// <summary>
        /// Taxeringsv�rde byggnad (SEK)
        /// </summary>
        [DataMember(Name="buildingValue",EmitDefaultValue=false)]
        public double BuildingValue { get; set; }
    
        /// <summary>
        /// Typkod
        /// </summary>
        [DataMember(Name="code",EmitDefaultValue=false)]
        public string Code { get; set; }
    
        /// <summary>
        /// Taxeringsv�rde totalt (SEK)
        /// </summary>
        [DataMember(Name="totalValue",EmitDefaultValue=false)]
        public double TotalValue { get; set; }
    
        /// <summary>
        /// Taxerings�r
        /// </summary>
        [DataMember(Name="year",EmitDefaultValue=false)]
        public int Year { get; set; }
    
    }
}