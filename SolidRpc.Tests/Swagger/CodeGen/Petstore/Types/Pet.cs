using System.Runtime.Serialization;
using SolidRpc.Tests.Swagger.CodeGen.Petstore.Types;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types {
    /// <summary>
    /// 
    /// </summary>
    public class Pet {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="id")]
        public long Id { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="category")]
        public Category Category { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="photoUrls")]
        public IEnumerable<string> PhotoUrls { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="tags")]
        public IEnumerable<Tag> Tags { get; set; }
    
        /// <summary>
        /// pet status in the store
        /// </summary>
        [DataMember(Name="status")]
        public string Status { get; set; }
    
    }
}