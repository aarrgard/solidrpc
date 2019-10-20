using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CondominiumInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Community {
        /// <summary>
        /// Andel i
        /// </summary>
        [DataMember(Name="shareIn",EmitDefaultValue=false)]
        public string ShareIn { get; set; }
    
        /// <summary>
        /// Beskrivning
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public string Description { get; set; }
    
        /// <summary>
        /// Utrymmen
        /// </summary>
        [DataMember(Name="space",EmitDefaultValue=false)]
        public string Space { get; set; }
    
    }
}