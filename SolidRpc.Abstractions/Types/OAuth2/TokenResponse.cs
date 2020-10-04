using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Abstractions.Types.OAuth2
{
    /// <summary>
    /// The response from a token request
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// REQUIRED.  The access token issued by the authorization server.
        /// </summary>
        [DataMember(Name = "access_token", EmitDefaultValue = false)]
        public string AccessToken { get; set; }

        /// <summary>
        /// REQUIRED.  The type of the token issued as described in Section 7.1.  Value is case insensitive.
        /// </summary>
        [DataMember(Name = "token_type", EmitDefaultValue = false)]
        public string TokenType { get; set; }

        /// <summary>
        /// RECOMMENDED.  The lifetime in seconds of the access token.  For example, the value &#39;3600&#39; denotes that the access token will expire in one hour from the time the response was generated. If omitted, the authorization server SHOULD provide the expiration time via other means or document the default value.
        /// </summary>
        [DataMember(Name = "expires_in", EmitDefaultValue = false)]
        public string ExpiresIn { get; set; }

        /// <summary>
        /// OPTIONAL.  The refresh token, which can be used to obtain new access tokens using the same authorization grant as describedin Section 6.
        /// </summary>
        [DataMember(Name = "refresh_token", EmitDefaultValue = false)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// OPTIONAL, if identical to the scope requested by the client; otherwise, REQUIRED.  The scope of the access token as described by Section 3.3.
        /// </summary>
        [DataMember(Name = "scope", EmitDefaultValue = false)]
        public IEnumerable<string> Scope { get; set; }

    }
}