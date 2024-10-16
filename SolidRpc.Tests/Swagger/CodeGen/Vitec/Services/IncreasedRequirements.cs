using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.IncreasedRequirement.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IncreasedRequirements {
        /// <summary>
        /// H&#228;mta ut&#246;kadekrav
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<IncreasedRequirementCollection>> IncreasedRequirementsGet(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}