using System.Threading.Tasks;
using System.Threading;
namespace SolidRpc.Security.Services.OAuth2.Microsoft {
    /// <summary>
    /// Defines logic for the callback from microsoft
    /// </summary>
    public interface IOAuth2MicrosoftCallback {
        /// <summary>
        /// Callback when a user has logged in successfully.
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task Login(
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Callback when a user has logged out successfully.
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task Logout(
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}