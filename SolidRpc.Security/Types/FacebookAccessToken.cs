using System.Runtime.Serialization;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// success
    /// </summary>
    public class FacebookAccessToken {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="access_token")]
        public string AccessToken { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="token_type")]
        public string TokenType { get; set; }
    
    }
}