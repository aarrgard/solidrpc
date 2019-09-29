using System.Runtime.Serialization;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Local.Types;
namespace SolidRpc.Tests.Swagger.CodeGen.Local.Types {
    /// <summary>
    /// Success
    /// </summary>
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
        public IEnumerable<ComplexObject1> Children { get; set; }
    
    }
}