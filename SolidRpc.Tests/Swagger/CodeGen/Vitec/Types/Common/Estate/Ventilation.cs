using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Ventilation {
        /// <summary>
        /// Typ av ventilation
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
        /// <summary>
        /// Besiktning
        /// </summary>
        [DataMember(Name="inspection",EmitDefaultValue=false)]
        public string Inspection { get; set; }
    
    }
}