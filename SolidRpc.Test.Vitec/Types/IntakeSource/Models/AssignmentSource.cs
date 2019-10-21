using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.IntakeSource.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AssignmentSource {
        /// <summary>
        /// id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Beskrivning p&#229; intagsk&#228;llan
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public string Description { get; set; }
    
    }
}