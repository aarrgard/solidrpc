using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Models.Api {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Email {
        /// <summary>
        /// Epost 1
        /// </summary>
        [DataMember(Name="emailAddress",EmitDefaultValue=false)]
        public string EmailAddress { get; set; }
    
        /// <summary>
        /// Epost 2
        /// </summary>
        [DataMember(Name="emailAddress2",EmitDefaultValue=false)]
        public string EmailAddress2 { get; set; }
    
    }
}