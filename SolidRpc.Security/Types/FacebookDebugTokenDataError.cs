using System.Runtime.Serialization;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// 
    /// </summary>
    public class FacebookDebugTokenDataError {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="code",EmitDefaultValue=false)]
        public long Code { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="message",EmitDefaultValue=false)]
        public string Message { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="subcode",EmitDefaultValue=false)]
        public long Subcode { get; set; }
    
    }
}