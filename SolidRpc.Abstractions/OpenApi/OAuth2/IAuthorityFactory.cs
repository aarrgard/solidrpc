using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.OAuth2
{
    /// <summary>
    /// Interface that provides access to an authority factory
    /// </summary>
    public interface IAuthorityFactory
    {
        /// <summary>
        /// Returns the principal for supplied jwt.
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="tokenChecks"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ClaimsPrincipal> GetPrincipalAsync(
            string jwt, 
            Action<IAuthorityTokenChecks> tokenChecks = null, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Returns the authority @ supplied url. The supplied string will be used to validate the issuer in the tokens.
        /// </summary>
        IAuthority GetAuthority(string authority);

        /// <summary>
        /// Returns the local authority @ supplied url. The supplied string will be used as the issuer when issuing tokens.
        /// </summary>
        IAuthorityLocal GetLocalAuthority(string authority);
    }
}
