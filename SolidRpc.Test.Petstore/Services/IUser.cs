using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Petstore.Types;
using System.Threading;
using System.Collections.Generic;
namespace SolidRpc.Test.Petstore.Services {
    /// <summary>
    /// Operations about user
    /// </summary>
    /// <a href="http://swagger.io">Find out more about our store</a>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IUser {
        /// <summary>
        /// Create user This can only be done by the logged in user.
        /// </summary>
        /// <param name="body">Created user object</param>
        /// <param name="cancellationToken"></param>
        Task CreateUser(
            User body,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Creates list of users with given input array
        /// </summary>
        /// <param name="body">List of user object</param>
        /// <param name="cancellationToken"></param>
        Task CreateUsersWithArrayInput(
            IEnumerable<User> body,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Creates list of users with given input array
        /// </summary>
        /// <param name="body">List of user object</param>
        /// <param name="cancellationToken"></param>
        Task CreateUsersWithListInput(
            IEnumerable<User> body,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Logs user into the system
        /// </summary>
        /// <param name="username">The user name for login</param>
        /// <param name="password">The password for login in clear text</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Petstore.Types.Services.User.LoginUser.InvalidUsernamePasswordSuppliedException">Invalid username/password supplied</exception>
        Task<string> LoginUser(
            string username,
            string password,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Logs out current logged in user session
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task LogoutUser(
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Delete user This can only be done by the logged in user.
        /// </summary>
        /// <param name="username">The name that needs to be deleted</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Petstore.Types.Services.User.DeleteUser.InvalidUsernameSuppliedException">Invalid username supplied</exception>
        /// <exception cref="SolidRpc.Test.Petstore.Types.Services.User.DeleteUser.UserNotFoundException">User not found</exception>
        Task DeleteUser(
            string username,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Get user by user name
        /// </summary>
        /// <param name="username">The name that needs to be fetched. Use user1 for testing.</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Petstore.Types.Services.User.GetUserByName.InvalidUsernameSuppliedException">Invalid username supplied</exception>
        /// <exception cref="SolidRpc.Test.Petstore.Types.Services.User.GetUserByName.UserNotFoundException">User not found</exception>
        Task<User> GetUserByName(
            string username,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Updated user This can only be done by the logged in user.
        /// </summary>
        /// <param name="username">name that need to be updated</param>
        /// <param name="body">Updated user object</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Petstore.Types.Services.User.UpdateUser.InvalidUserSuppliedException">Invalid user supplied</exception>
        /// <exception cref="SolidRpc.Test.Petstore.Types.Services.User.UpdateUser.UserNotFoundException">User not found</exception>
        Task UpdateUser(
            string username,
            User body,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}