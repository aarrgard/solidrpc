using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Category.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Categories {
        /// <summary>
        /// F&#246;retagskategorier
        /// </summary>
        [DataMember(Name="companyCategories",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Category.Models.Category> CompanyCategories { get; set; }
    
        /// <summary>
        /// Personkategorier
        /// </summary>
        [DataMember(Name="personCategories",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Category.Models.Category> PersonCategories { get; set; }
    
    }
}