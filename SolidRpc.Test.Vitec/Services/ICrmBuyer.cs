using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.Buyer.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmBuyer {
        /// <summary>
        /// Bank- och f�rs�kringsppgifter
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="contactId"></param>
        /// <param name="estateId"></param>
        /// <param name="cancellationToken"></param>
        Task<BuyerEconomy> CrmBuyerGetEconomy(
            string customerId,
            string contactId,
            string estateId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}