using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CustomField.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class FieldValueCriteria {
        /// <summary>
        /// F�ltnamn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// V�rde
        /// </summary>
        [DataMember(Name="value",EmitDefaultValue=false)]
        public string Value { get; set; }
    
    }
}