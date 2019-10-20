using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPublicAdvertisingPlot {
        /// <summary>
        /// H�mta tomt
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">Tomtens id</param>
        /// <param name="cancellationToken"></param>
        Task<Plot> PublicAdvertisingPlotGet(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}