using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models = SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Area.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IArea {
        /// <summary>
        /// H�mtar en lista �ver omr�den.
        /// </summary>
        /// <param name="criteriaCustomerId"></param>
        /// <param name="criteriaAreaId">Omr�dets id</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.Area>> AreaGetAreas(
            string criteriaCustomerId,
            string criteriaAreaId = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}