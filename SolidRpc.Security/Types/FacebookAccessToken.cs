using System.Runtime.Serialization;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// success
    /// </summary>
    public class FacebookAccessToken {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="access_token",EmitDefaultValue=false)]
        public string AccessToken { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="token_type",EmitDefaultValue=false)]
        public string TokenType { get; set; }
    
    }
}