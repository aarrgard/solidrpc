using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Estate.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Tenant {
        /// <summary>
        /// Id p&#229; hyresg&#228;sten
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Typ av hyresg&#228;st
        /// </summary>
        [DataMember(Name="contactType",EmitDefaultValue=false)]
        public string ContactType { get; set; }
    
    }
}