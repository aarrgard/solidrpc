using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class FarmInspection {
        /// <summary>
        /// Besiktningsf&#246;retag
        /// </summary>
        [DataMember(Name="company",EmitDefaultValue=false)]
        public string Company { get; set; }
    
        /// <summary>
        /// K&#246;pargenomg&#229;ng genomf&#246;rd
        /// </summary>
        [DataMember(Name="buyersInspection",EmitDefaultValue=false)]
        public bool? BuyersInspection { get; set; }
    
        /// <summary>
        /// Besiktning betald av
        /// </summary>
        [DataMember(Name="paidBy",EmitDefaultValue=false)]
        public string PaidBy { get; set; }
    
        /// <summary>
        /// F&#246;rbesiktigad
        /// </summary>
        [DataMember(Name="inspected",EmitDefaultValue=false)]
        public bool? Inspected { get; set; }
    
        /// <summary>
        /// S&#228;ljarf&#246;rs&#228;kring
        /// </summary>
        [DataMember(Name="sellerInsurence",EmitDefaultValue=false)]
        public bool? SellerInsurence { get; set; }
    
    }
}