using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Lead.Models;
using System.Threading;
using Models = SolidRpc.Test.Vitec.Types.Lead.Models;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmLead {
        /// <summary>
        /// H�mtar en lista �ver leadsk�llor.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<LeadSource>> CrmLeadGetLeadSources(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Skapar ett lead
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="lead"></param>
        /// <param name="cancellationToken"></param>
        Task<string> CrmLeadCreate(
            string customerId,
            Lead lead,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}