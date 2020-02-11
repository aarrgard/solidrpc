using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PublicAdvertisingEstateStatusInfo {
        /// <summary>
        /// Meddelande till anv&#228;ndaren
        /// </summary>
        [DataMember(Name="message",EmitDefaultValue=false)]
        public string Message { get; set; }
    
        /// <summary>
        /// Typ, anger hur meddelandet presenteras, se v&#228;rden f&#246;r vad det inneb&#228;r.
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
    }
}