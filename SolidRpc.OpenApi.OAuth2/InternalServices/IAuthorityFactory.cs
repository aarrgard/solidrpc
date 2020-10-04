using System;

namespace SolidRpc.OpenApi.OAuth2.InternalServices
{
    /// <summary>
    /// Interface that provides access to an authority factory
    /// </summary>
    public interface IAuthorityFactory
    {
        /// <summary>
        /// Returns the authority @ supplied url
        /// </summary>
        IAuthority GetAuthority(Uri url);
    }
}
