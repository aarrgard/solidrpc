using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Services;
using System.Security.Claims;
using System.Security.Principal;

[assembly: SolidRpcServiceAttribute(typeof(ISolidRpcAuthorization), typeof(SolidRpcAuthorization), SolidRpcServiceLifetime.Scoped)]
namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Implements the solid authorization logic
    /// </summary>
    public class SolidRpcAuthorization : ISolidRpcAuthorization
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public SolidRpcAuthorization()
        {
            var claims = new Claim[] { };
            CurrentPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        }

        /// <summary>
        /// The current principal
        /// </summary>
        public IPrincipal CurrentPrincipal { get; set; }
    }
}
