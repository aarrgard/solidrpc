using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// success
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
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