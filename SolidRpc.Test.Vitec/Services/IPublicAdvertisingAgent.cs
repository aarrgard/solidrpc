using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPublicAdvertisingAgent {
        /// <summary>
        /// H&#228;mtar m&#228;klarelista.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<EstateAgent>> PublicAdvertisingAgentGetList(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar m&#228;klare.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">Mäklarens id</param>
        /// <param name="cancellationToken"></param>
        Task<EstateAgent> PublicAdvertisingAgentGet(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}