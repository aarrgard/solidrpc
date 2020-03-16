using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models = SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Municipality.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IMunicipality {
        /// <summary>
        /// H&#228;mtar en lista &#246;ver kommuner.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">Kommunid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.Municipality>> MunicipalityGetMunicipalities(
            string customerId,
            string id = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}