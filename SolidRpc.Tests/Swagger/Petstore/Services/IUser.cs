
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.Petstore.Services;
using SolidRpc.Tests.Swagger.Petstore.Types;
namespace SolidRpc.Tests.Swagger.Petstore.Services {
    public interface IUser {
        /// <summary>
        /// Create user This can only be done by the logged in user.
        /// </summary>
        /// <param name="body">Created user object</param>
        /// <param name="cancellationToken"></param>
        Task CreateUser(
            User body,
            CancellationToken cancellationToken);
    
        /// <summary>
        /// Creates list of users with given input array
        /// </summary>
        /// <param name="body">List of user object</param>
        /// <param name="cancellationToken"></param>
        Task CreateUsersWithArrayInput(
            IEnumerable<User> body,
            CancellationToken cancellationToken);
    
        /// <summary>
        /// Creates list of users with given input array
        /// </summary>
        /// <param name="body">List of user object</param>
        /// <param name="cancellationToken"></param>
        Task CreateUsersWithListInput(
            IEnumerable<User> body,
            CancellationToken cancellationToken);
    
        /// <summary>
        /// Logs user into the system
        /// </summary>
        /// <param name="username">The user name for login</param>
        /// <param name="password">The password for login in clear text</param>
        /// <param name="cancellationToken"></param>
        Task<string> LoginUser(
            string username,
            string password,
            CancellationToken cancellationToken);
    
        /// <summary>
        /// Logs out current logged in user session
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task LogoutUser(
            CancellationToken cancellationToken);
    
        /// <summary>
        /// Delete user This can only be done by the logged in user.
        /// </summary>
        /// <param name="username">The name that needs to be deleted</param>
        /// <param name="cancellationToken"></param>
        Task DeleteUser(
            string username,
            CancellationToken cancellationToken);
    
        /// <summary>
        /// Get user by user name
        /// </summary>
        /// <param name="username">The name that needs to be fetched. Use user1 for testing.</param>
        /// <param name="cancellationToken"></param>
        Task<User> GetUserByName(
            string username,
            CancellationToken cancellationToken);
    
        /// <summary>
        /// Updated user This can only be done by the logged in user.
        /// </summary>
        /// <param name="username">name that need to be updated</param>
        /// <param name="body">Updated user object</param>
        /// <param name="cancellationToken"></param>
        Task UpdateUser(
            string username,
            User body,
            CancellationToken cancellationToken);
    
    }
}