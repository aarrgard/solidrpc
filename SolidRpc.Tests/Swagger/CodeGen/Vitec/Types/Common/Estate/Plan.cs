using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Plan {
        /// <summary>
        /// Typ
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
        /// <summary>
        /// Genomf&#246;randetid
        /// </summary>
        [DataMember(Name="implemetationPeriod",EmitDefaultValue=false)]
        public string ImplemetationPeriod { get; set; }
    
    }
}