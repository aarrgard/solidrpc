using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.CustomField.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CustomFields {
        /// <summary>
        /// Kund id
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Lista av egendefinerade f�lt.
        /// </summary>
        [DataMember(Name="fields",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.CustomField.Models.Field> Fields { get; set; }
    
    }
}