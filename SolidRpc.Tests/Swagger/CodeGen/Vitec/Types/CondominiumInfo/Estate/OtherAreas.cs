using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CondominiumInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class OtherAreas {
        /// <summary>
        /// Sammanst�llning
        /// </summary>
        [DataMember(Name="compilation",EmitDefaultValue=false)]
        public string Compilation { get; set; }
    
        /// <summary>
        /// G�rdsplats/inneg�rd
        /// </summary>
        [DataMember(Name="courtyard",EmitDefaultValue=false)]
        public string Courtyard { get; set; }
    
        /// <summary>
        /// St�dning
        /// </summary>
        [DataMember(Name="cleaning",EmitDefaultValue=false)]
        public string Cleaning { get; set; }
    
    }
}