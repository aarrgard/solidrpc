using System.Runtime.Serialization;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// 
    /// </summary>
    public class FacebookDebugTokenDataError {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="code")]
        public long Code { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="message")]
        public string Message { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="subcode")]
        public long Subcode { get; set; }
    
    }
}