using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PartnerService.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ILead {
        /// <summary>
        /// L�gg till ett lead f�r en person
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="personLead">Personlead</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.Lead.LeadAddPersonLead.NoContentException">No Content</exception>
        Task LeadAddPersonLead(
            string customerId,
            PersonLead personLead,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}