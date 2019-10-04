using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Security.Front.InternalServices
{
    /// <summary>
    /// Represents a security token
    /// </summary>
    public interface IAccessToken
    {
        /// <summary>
        /// Returns the accees token.
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        /// The token type
        /// </summary>
        string TokenType { get; }

        /// <summary>
        /// The number of seconds until expiration
        /// </summary>
        int ExpiresInSeconds { get; }
    }
}
