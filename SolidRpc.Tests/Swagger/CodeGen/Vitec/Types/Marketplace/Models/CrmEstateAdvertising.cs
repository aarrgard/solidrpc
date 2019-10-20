using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Marketplace.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CrmEstateAdvertising {
        /// <summary>
        /// Bostadens marknadsplatser
        /// </summary>
        [DataMember(Name="marketplaceIds",EmitDefaultValue=false)]
        public IEnumerable<int> MarketplaceIds { get; set; }
    
    }
}