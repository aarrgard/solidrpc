using SolidRpc.Security.Types;
using System.Runtime.Serialization;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// success
    /// </summary>
    public class FacebookDebugToken {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="data",EmitDefaultValue=false)]
        public FacebookDebugTokenData Data { get; set; }
    
    }
}