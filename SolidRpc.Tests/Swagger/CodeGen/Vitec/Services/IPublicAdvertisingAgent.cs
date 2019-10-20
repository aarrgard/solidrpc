using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPublicAdvertisingAgent {
        /// <summary>
        /// H�mtar m�klarelista.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<EstateAgent>> PublicAdvertisingAgentGetList(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar m�klare.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">M�klarens id</param>
        /// <param name="cancellationToken"></param>
        Task<EstateAgent> PublicAdvertisingAgentGet(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}