using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// success
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class FacebookDebugToken {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="data",EmitDefaultValue=false)]
        public SolidRpc.Security.Types.FacebookDebugTokenData Data { get; set; }
    
    }
}