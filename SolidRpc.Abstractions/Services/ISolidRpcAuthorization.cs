using System.Security.Claims;

namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Implements the logic for authorization
    /// </summary>
    public interface ISolidRpcAuthorization
    {
        /// <summary>
        /// Gets / sets the current principal
        /// </summary>
        ClaimsPrincipal CurrentPrincipal { get; set; }

        /// <summary>
        /// Returns the "client_id" claim from current principal
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// Returns the "session_id" claim from current principal
        /// </summary>
        string SessionId { get; }
    }
}
