using System.Runtime.Serialization;
using SolidRpc.Security.Types;
using System.Collections.Generic;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// 
    /// </summary>
    public class FacebookDebugTokenData {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="app_id")]
        public long AppId { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="type")]
        public string Type { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="application")]
        public string Application { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="data_access_expires_at")]
        public long DataAccessExpiresAt { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="error")]
        public FacebookDebugTokenDataError Error { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="expires_at")]
        public long ExpiresAt { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="is_valid")]
        public bool IsValid { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="scopes")]
        public IEnumerable<string> Scopes { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="user_id")]
        public long UserId { get; set; }
    
    }
}