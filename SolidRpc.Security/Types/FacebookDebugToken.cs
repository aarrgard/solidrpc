using System.CodeDom.Compiler;
using SolidRpc.Security.Types;
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
        public FacebookDebugTokenData Data { get; set; }
    
    }
}