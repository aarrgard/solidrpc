using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Petstore.Types {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
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
        public SolidRpc.Test.Petstore.Types.Category Category { get; set; }
    
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
        public IEnumerable<SolidRpc.Test.Petstore.Types.Tag> Tags { get; set; }
    
        /// <summary>
        /// pet status in the store
        /// </summary>
        [DataMember(Name="status",EmitDefaultValue=false)]
        public string Status { get; set; }
    
    }
}