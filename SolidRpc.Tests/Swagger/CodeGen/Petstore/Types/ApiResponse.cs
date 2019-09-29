using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types {
    /// <summary>
    /// successful operation
    /// </summary>
    public class ApiResponse {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="code",EmitDefaultValue=false)]
        public int Code { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="message",EmitDefaultValue=false)]
        public string Message { get; set; }
    
    }
}