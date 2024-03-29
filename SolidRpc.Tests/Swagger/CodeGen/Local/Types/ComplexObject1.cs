using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Local.Types {
    /// <summary>
    /// Success
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ComplexObject1 {
        /// <summary>
        /// Value1
        /// </summary>
        [DataMember(Name="value1",EmitDefaultValue=false)]
        public string Value1 { get; set; }
    
        /// <summary>
        /// Value2
        /// </summary>
        [DataMember(Name="value2",EmitDefaultValue=false)]
        public string Value2 { get; set; }
    
        /// <summary>
        /// The children
        /// </summary>
        [DataMember(Name="children",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Local.Types.ComplexObject1> Children { get; set; }
    
    }
}