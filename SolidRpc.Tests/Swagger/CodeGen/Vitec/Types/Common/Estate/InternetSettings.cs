using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class InternetSettings {
        /// <summary>
        /// Valt alternativ f�r bud
        /// </summary>
        [DataMember(Name="bidSetting",EmitDefaultValue=false)]
        public string BidSetting { get; set; }
    
    }
}