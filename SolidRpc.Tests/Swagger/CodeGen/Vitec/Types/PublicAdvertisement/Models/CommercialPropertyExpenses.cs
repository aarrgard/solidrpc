using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CommercialPropertyExpenses {
        /// <summary>
        /// Driftskostnad per &#229;r
        /// </summary>
        [DataMember(Name="operatingCost",EmitDefaultValue=false)]
        public double? OperatingCost { get; set; }
    
    }
}