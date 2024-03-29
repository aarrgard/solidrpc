using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models = SolidRpc.Test.Vitec.Types.Area.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IArea {
        /// <summary>
        /// H&#228;mtar en lista &#246;ver omr&#229;den.
        /// </summary>
        /// <param name="criteriaCustomerId"></param>
        /// <param name="criteriaAreaId">Områdets id</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.Area>> AreaGetAreas(
            string criteriaCustomerId,
            string criteriaAreaId = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}