using System.Threading.Tasks;
using SolidRpc.Security.Types;
using System.Threading;
namespace SolidRpc.Security.Services.Google {
    /// <summary>
    /// Defines logic for the callback from google
    /// </summary>
    public interface IGoogleLocal {
        /// <summary>
        /// Returns the login provider information
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<LoginProvider> LoginProvider(
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Returns the script to embed to enable login
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<WebContent> LoginScript(
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Callback when a user has logged in successfully.
        /// </summary>
        /// <param name="accessToken">The the access token for the logged in user</param>
        /// <param name="cancellationToken"></param>
        Task<string> LoggedIn(
            string accessToken,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Callback when a user has logged out successfully.
        /// </summary>
        /// <param name="accessToken">The the access token for the logged out in user</param>
        /// <param name="cancellationToken"></param>
        Task<string> LoggedOut(
            string accessToken,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}