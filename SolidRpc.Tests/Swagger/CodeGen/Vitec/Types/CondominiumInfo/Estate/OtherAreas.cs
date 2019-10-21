using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CondominiumInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class OtherAreas {
        /// <summary>
        /// Sammanst&#228;llning
        /// </summary>
        [DataMember(Name="compilation",EmitDefaultValue=false)]
        public string Compilation { get; set; }
    
        /// <summary>
        /// G&#229;rdsplats/inneg&#229;rd
        /// </summary>
        [DataMember(Name="courtyard",EmitDefaultValue=false)]
        public string Courtyard { get; set; }
    
        /// <summary>
        /// St&#228;dning
        /// </summary>
        [DataMember(Name="cleaning",EmitDefaultValue=false)]
        public string Cleaning { get; set; }
    
    }
}