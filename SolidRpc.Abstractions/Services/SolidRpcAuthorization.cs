using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Services;
using System;
using System.Security.Claims;

[assembly: SolidRpcServiceAttribute(typeof(ISolidRpcAuthorization), typeof(SolidRpcAuthorization), SolidRpcServiceLifetime.Scoped)]
namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Implements the solid authorization logic
    /// </summary>
    public class SolidRpcAuthorization : ISolidRpcAuthorization
    {
        private ClaimsPrincipal _currentPrincipal;

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public SolidRpcAuthorization()
        {
            _currentPrincipal = new ClaimsPrincipal();
        }

        /// <summary>
        /// The current principal
        /// </summary>
        public ClaimsPrincipal CurrentPrincipal { 
            get => _currentPrincipal;
            set => _currentPrincipal = value ?? new ClaimsPrincipal();
        }
    }
}
