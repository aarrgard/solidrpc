using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Seller.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmSeller {
        /// <summary>
        /// Bankuppgifter
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="contactId"></param>
        /// <param name="estateId"></param>
        /// <param name="cancellationToken"></param>
        Task<SellerEconomy> CrmSellerGetEconomy(
            string customerId,
            string contactId,
            string estateId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}