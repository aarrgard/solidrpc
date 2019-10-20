using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class TVAndBroadband {
        /// <summary>
        /// TV
        /// </summary>
        [DataMember(Name="tv",EmitDefaultValue=false)]
        public string Tv { get; set; }
    
        /// <summary>
        /// Bredband
        /// </summary>
        [DataMember(Name="broadband",EmitDefaultValue=false)]
        public string Broadband { get; set; }
    
    }
}