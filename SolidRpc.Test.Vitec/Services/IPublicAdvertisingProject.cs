using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPublicAdvertisingProject {
        /// <summary>
        /// H�mta projekt
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">Projektets id</param>
        /// <param name="cancellationToken"></param>
        Task<Project> PublicAdvertisingProjectGet(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}