using System;

namespace SolidRpc.Abstractions.OpenApi.OAuth2
{
    /// <summary>
    /// Interface that provides access to an authority factory
    /// </summary>
    public interface IAuthorityFactory
    {
        /// <summary>
        /// Returns the authority @ supplied url
        /// </summary>
        IAuthority GetAuthority(Uri authority);

        /// <summary>
        /// Returns the local authority @ supplied url
        /// </summary>
        IAuthorityLocal GetLocalAuthority(Uri authority);
    }
}
