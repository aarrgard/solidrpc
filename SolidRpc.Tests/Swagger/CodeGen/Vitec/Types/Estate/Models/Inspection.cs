using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Inspection {
        /// <summary>
        /// F&#246;rbesiktigad
        /// </summary>
        [DataMember(Name="inspected",EmitDefaultValue=false)]
        public bool Inspected { get; set; }
    
        /// <summary>
        /// S&#228;ljarf&#246;rs&#228;kring
        /// </summary>
        [DataMember(Name="sellerInsurence",EmitDefaultValue=false)]
        public bool SellerInsurence { get; set; }
    
    }
}