using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class FarmInspection {
        /// <summary>
        /// Besiktningsf�retag
        /// </summary>
        [DataMember(Name="company",EmitDefaultValue=false)]
        public string Company { get; set; }
    
        /// <summary>
        /// K�pargenomg�ng genomf�rd
        /// </summary>
        [DataMember(Name="buyersInspection",EmitDefaultValue=false)]
        public bool BuyersInspection { get; set; }
    
        /// <summary>
        /// Besiktning betald av
        /// </summary>
        [DataMember(Name="paidBy",EmitDefaultValue=false)]
        public string PaidBy { get; set; }
    
        /// <summary>
        /// F�rbesiktigad
        /// </summary>
        [DataMember(Name="inspected",EmitDefaultValue=false)]
        public bool Inspected { get; set; }
    
        /// <summary>
        /// S�ljarf�rs�kring
        /// </summary>
        [DataMember(Name="sellerInsurence",EmitDefaultValue=false)]
        public bool SellerInsurence { get; set; }
    
    }
}