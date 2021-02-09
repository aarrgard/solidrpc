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
    }
}
