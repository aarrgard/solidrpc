using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models = SolidRpc.Test.Vitec.Types.Municipality.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IMunicipality {
        /// <summary>
        /// H�mtar en lista �ver kommuner.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">Kommunid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.Municipality>> MunicipalityGetMunicipalities(
            string customerId,
            string id = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}