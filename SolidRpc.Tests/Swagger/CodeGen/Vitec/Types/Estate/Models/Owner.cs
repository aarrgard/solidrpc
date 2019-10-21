using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Owner {
        /// <summary>
        /// Id p&#229; &#228;garen/uthyraren
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Typ av kontakt
        /// </summary>
        [DataMember(Name="contactType",EmitDefaultValue=false)]
        public string ContactType { get; set; }
    
    }
}