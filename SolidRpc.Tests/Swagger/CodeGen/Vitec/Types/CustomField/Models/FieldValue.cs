using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CustomField.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class FieldValue {
        /// <summary>
        /// Etikett
        /// </summary>
        [DataMember(Name="label",EmitDefaultValue=false)]
        public string Label { get; set; }
    
        /// <summary>
        /// F&#228;ltnamn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// V&#228;rde
        /// </summary>
        [DataMember(Name="value",EmitDefaultValue=false)]
        public string Value { get; set; }
    
    }
}