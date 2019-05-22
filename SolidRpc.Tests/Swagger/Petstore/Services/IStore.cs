
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.Petstore.Services;
using SolidRpc.Tests.Swagger.Petstore.Types;
namespace SolidRpc.Tests.Swagger.Petstore.Services {
    public interface IStore {
        /// <summary>
        /// Returns pet inventories by status Returns a map of status codes to quantities
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<GetInventory200> GetInventory(
            CancellationToken cancellationToken);
    
        /// <summary>
        /// Place an order for a pet
        /// </summary>
        /// <param name="body">order placed for purchasing the pet</param>
        /// <param name="cancellationToken"></param>
        Task<Order> PlaceOrder(
            Order body,
            CancellationToken cancellationToken);
    
        /// <summary>
        /// Delete purchase order by ID For valid response try integer IDs with positive integer value.\ \ Negative or non-integer values will generate API errors
        /// </summary>
        /// <param name="orderId">ID of the order that needs to be deleted</param>
        /// <param name="cancellationToken"></param>
        Task DeleteOrder(
            long orderId,
            CancellationToken cancellationToken);
    
        /// <summary>
        /// Find purchase order by ID For valid response try integer IDs with value >= 1 and <= 10.\ \ Other values will generated exceptions
        /// </summary>
        /// <param name="orderId">ID of pet that needs to be fetched</param>
        /// <param name="cancellationToken"></param>
        Task<Order> GetOrderById(
            long orderId,
            CancellationToken cancellationToken);
    
    }
}