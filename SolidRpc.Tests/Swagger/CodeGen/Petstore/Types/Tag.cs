using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Tag {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public long? Id { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
    }
}