using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PartnerService.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ContactAgent {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Externt id
        /// </summary>
        [DataMember(Name="externalId",EmitDefaultValue=false)]
        public string ExternalId { get; set; }
    
        /// <summary>
        /// E-postadress
        /// </summary>
        [DataMember(Name="email",EmitDefaultValue=false)]
        public string Email { get; set; }
    
    }
}