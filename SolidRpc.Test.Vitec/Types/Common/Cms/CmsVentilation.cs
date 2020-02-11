using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Common.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsVentilation {
        /// <summary>
        /// Typ av ventilation
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
    }
}