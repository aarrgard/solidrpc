using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPublicAdvertisingCondominium {
        /// <summary>
        /// H&#228;mta &#228;gander&#228;tt
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">Bostadens id</param>
        /// <param name="cancellationToken"></param>
        Task<Condominium> PublicAdvertisingCondominiumGet(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}