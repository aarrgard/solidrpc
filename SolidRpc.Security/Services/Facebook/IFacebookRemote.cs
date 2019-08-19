using System.Threading.Tasks;
using SolidRpc.Security.Types;
using System.Threading;
namespace SolidRpc.Security.Services.Facebook {
    /// <summary>
    /// Defines logic @ facebook
    /// </summary>
    public interface IFacebookRemote {
        /// <summary>
        /// Obtains an access token for the application
        /// </summary>
        /// <param name="clientId">The app id</param>
        /// <param name="clientSecret">The app secret</param>
        /// <param name="grantType">The grant type - client_credentials</param>
        /// <param name="cancellationToken"></param>
        Task<FacebookAccessToken> GetAccessToken(
            string clientId,
            string clientSecret,
            string grantType,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Returns information about supplied access token
        /// </summary>
        /// <param name="inputToken">The token to get the information about</param>
        /// <param name="accessToken">The access token</param>
        /// <param name="cancellationToken"></param>
        Task<FacebookDebugToken> GetDebugToken(
            string inputToken,
            string accessToken,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}