using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Store.GetInventory;
using SolidRpc.Tests.Swagger.CodeGen.Petstore.Security;
using System.Threading;
using SolidRpc.Tests.Swagger.CodeGen.Petstore.Types;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Services {
    /// <summary>
    /// Access to Petstore orders
    /// </summary>
    public interface IStore {
        /// <summary>
        /// Returns pet inventories by status Returns a map of status codes to quantities
        /// </summary>
        /// <param name="cancellationToken"></param>
        [ApiKey]
        Task<Response200> GetInventory(
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Place an order for a pet
        /// </summary>
        /// <param name="body">order placed for purchasing the pet</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Store.PlaceOrder.InvalidOrderException">Invalid Order</exception>
        Task<Order> PlaceOrder(
            Order body,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Delete purchase order by ID For valid response try integer IDs with positive integer value.\ \ Negative or non-integer values will generate API errors
        /// </summary>
        /// <param name="orderId">ID of the order that needs to be deleted</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Store.DeleteOrder.InvalidIDSuppliedException">Invalid ID supplied</exception>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Store.DeleteOrder.OrderNotFoundException">Order not found</exception>
        Task DeleteOrder(
            long orderId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Find purchase order by ID For valid response try integer IDs with value &gt;= 1 and &lt;= 10.\ \ Other values will generated exceptions
        /// </summary>
        /// <param name="orderId">ID of pet that needs to be fetched</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Store.GetOrderById.InvalidIDSuppliedException">Invalid ID supplied</exception>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Petstore.Types.Services.Store.GetOrderById.OrderNotFoundException">Order not found</exception>
        Task<Order> GetOrderById(
            long orderId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}