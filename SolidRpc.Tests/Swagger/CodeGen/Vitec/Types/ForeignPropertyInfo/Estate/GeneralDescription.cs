using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ForeignPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class GeneralDescription {
        /// <summary>
        /// Spr&#229;k
        /// </summary>
        [DataMember(Name="language",EmitDefaultValue=false)]
        public string Language { get; set; }
    
        /// <summary>
        /// Text
        /// </summary>
        [DataMember(Name="text",EmitDefaultValue=false)]
        public string Text { get; set; }
    
    }
}