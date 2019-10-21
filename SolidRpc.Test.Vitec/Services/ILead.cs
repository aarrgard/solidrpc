using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.PartnerService.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ILead {
        /// <summary>
        /// L&#228;gg till ett lead f&#246;r en person
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="personLead">Personlead</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.Lead.LeadAddPersonLead.NoContentException">No Content</exception>
        Task LeadAddPersonLead(
            string customerId,
            PersonLead personLead,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}