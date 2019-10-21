using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPublicAdvertisingProject {
        /// <summary>
        /// H&#228;mta projekt
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