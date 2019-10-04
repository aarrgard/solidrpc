using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Security.Types;
using System.Collections.Generic;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class FacebookDebugTokenData {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="app_id",EmitDefaultValue=false)]
        public long AppId { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="application",EmitDefaultValue=false)]
        public string Application { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="data_access_expires_at",EmitDefaultValue=false)]
        public long DataAccessExpiresAt { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="error",EmitDefaultValue=false)]
        public FacebookDebugTokenDataError Error { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="expires_at",EmitDefaultValue=false)]
        public long ExpiresAt { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="is_valid",EmitDefaultValue=false)]
        public bool IsValid { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="scopes",EmitDefaultValue=false)]
        public IEnumerable<string> Scopes { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="user_id",EmitDefaultValue=false)]
        public long UserId { get; set; }
    
    }
}