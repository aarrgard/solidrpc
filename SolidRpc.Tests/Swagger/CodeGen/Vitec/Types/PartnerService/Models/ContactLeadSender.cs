using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PartnerService.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ContactLeadSender {
        /// <summary>
        /// Anv�ndare
        /// </summary>
        [DataMember(Name="user",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PartnerService.Models.UserLeadSender User { get; set; }
    
        /// <summary>
        /// F�retag
        /// </summary>
        [DataMember(Name="company",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PartnerService.Models.CompanyLeadSender Company { get; set; }
    
    }
}