using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Client {
        /// <summary>
        /// The id of the client
        /// </summary>
        [DataMember(Name="client_id",EmitDefaultValue=false)]
        public string ClientId { get; set; }
    
        /// <summary>
        /// The name of the client
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
    }
}