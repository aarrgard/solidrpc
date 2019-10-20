using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.Order.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IOrder {
        /// <summary>
        /// Uppdatera status p� en tj�nst
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="orderStatus"></param>
        /// <param name="cancellationToken"></param>
        Task<bool> OrderUpdateStatus(
            string customerId,
            OrderStatus orderStatus,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Beg�r anv�ndarinteraktion p� en tj�nst
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        Task<bool> OrderUserInteraction(
            string customerId,
            OrderUserInteractionRequest request,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}