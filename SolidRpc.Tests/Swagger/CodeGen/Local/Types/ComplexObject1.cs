using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Local.Types {
    /// <summary>
    /// Success
    /// </summary>
    public class ComplexObject1 {
        /// <summary>
        /// Value1
        /// </summary>
        public string Value1 { get; set; }
    
        /// <summary>
        /// Value2
        /// </summary>
        public string Value2 { get; set; }
    
        /// <summary>
        /// The children
        /// </summary>
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Local.Types.ComplexObject1> Children { get; set; }
    
    }
}