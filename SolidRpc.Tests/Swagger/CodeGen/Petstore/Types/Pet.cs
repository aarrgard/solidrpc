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
        [DataMember(Name="id",EmitDefaultValue=false)]
        public long Id { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="category",EmitDefaultValue=false)]
        public Category Category { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="photoUrls",EmitDefaultValue=false)]
        public IEnumerable<string> PhotoUrls { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="tags",EmitDefaultValue=false)]
        public IEnumerable<Tag> Tags { get; set; }
    
        /// <summary>
        /// pet status in the store
        /// </summary>
        [DataMember(Name="status",EmitDefaultValue=false)]
        public string Status { get; set; }
    
    }
}