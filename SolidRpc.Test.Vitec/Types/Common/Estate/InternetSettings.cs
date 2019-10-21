using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class InternetSettings {
        /// <summary>
        /// Valt alternativ f&#246;r bud
        /// </summary>
        [DataMember(Name="bidSetting",EmitDefaultValue=false)]
        public string BidSetting { get; set; }
    
    }
}