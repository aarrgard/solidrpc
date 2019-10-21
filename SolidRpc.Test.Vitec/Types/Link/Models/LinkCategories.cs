using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.Link.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class LinkCategories {
        /// <summary>
        /// Kundid
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Lista av l&#228;nkkategorier
        /// </summary>
        [DataMember(Name="categories",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Link.Models.Category> Categories { get; set; }
    
    }
}