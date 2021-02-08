using System;

namespace SolidRpc.Abstractions.OpenApi.OAuth2
{
    /// <summary>
    /// Interface that provides access to an authority factory
    /// </summary>
    public interface IAuthorityFactory
    {
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
