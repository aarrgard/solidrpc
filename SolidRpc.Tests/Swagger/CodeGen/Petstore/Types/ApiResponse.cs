using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types {
    /// <summary>
    /// successful operation
    /// </summary>
    public class ApiResponse {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="code")]
        public int Code { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="type")]
        public string Type { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="message")]
        public string Message { get; set; }
    
    }
}