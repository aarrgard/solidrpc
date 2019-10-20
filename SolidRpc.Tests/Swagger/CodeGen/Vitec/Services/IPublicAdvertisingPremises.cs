using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPublicAdvertisingPremises {
        /// <summary>
        /// H�mta lokal
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">Lokalens id</param>
        /// <param name="cancellationToken"></param>
        Task<Premises> PublicAdvertisingPremisesGet(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}