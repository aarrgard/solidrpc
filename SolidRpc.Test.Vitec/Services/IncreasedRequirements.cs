using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.IncreasedRequirement.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IncreasedRequirements {
        /// <summary>
        /// H�mta ut�kadekrav
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<IncreasedRequirementCollection>> IncreasedRequirementsGet(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}