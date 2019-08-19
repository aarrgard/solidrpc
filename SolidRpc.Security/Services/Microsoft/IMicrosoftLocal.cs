using System.Threading.Tasks;
using SolidRpc.Security.Types;
using System.Threading;
namespace SolidRpc.Security.Services.Microsoft {
    /// <summary>
    /// Defines logic for the callback from microsoft
    /// </summary>
    public interface IMicrosoftLocal {
        /// <summary>
        /// Returns the login provider information
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<LoginProvider> LoginProvider(
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Returns the script to embedd to enable login
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<WebContent> LoginScript(
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Callback when a user has logged in successfully.
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task LoggedIn(
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Callback when a user has logged out successfully.
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task LoggedOut(
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}