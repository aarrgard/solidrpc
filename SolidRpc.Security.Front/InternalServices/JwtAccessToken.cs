using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace SolidRpc.Security.Front.InternalServices
{
    /// <summary>
    /// Represents a jwt access token.
    /// </summary>
    public class JwtAccessToken : IAccessToken
    {
        public JwtAccessToken(JwtSecurityToken jwtSecurityToken)
        {
            JwtSecurityToken = jwtSecurityToken;
        }

        public string AccessToken => JwtSecurityToken.RawData;

        public string TokenType => "jwt";

        public int ExpiresInSeconds => (int)(JwtSecurityToken.ValidTo - DateTime.UtcNow).TotalSeconds;

        private JwtSecurityToken JwtSecurityToken { get; }
    }
}