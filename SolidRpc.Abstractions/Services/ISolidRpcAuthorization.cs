using System.Security.Principal;

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
        IPrincipal CurrentPrincipal { get; set; }
    }
}
