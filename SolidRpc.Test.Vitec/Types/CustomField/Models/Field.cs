using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.CustomField.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Field {
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
        /// F&#228;lttyp
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
        /// <summary>
        /// Giltilig f&#246;r
        /// </summary>
        [DataMember(Name="validFor",EmitDefaultValue=false)]
        public IEnumerable<string> ValidFor { get; set; }
    
    }
}