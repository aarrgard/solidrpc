using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types {
    /// <summary>
    /// 
    /// </summary>
    public class Category {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="id")]
        public long Id { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }
    
    }
}