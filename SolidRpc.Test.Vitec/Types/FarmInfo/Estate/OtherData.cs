using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.FarmInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class OtherData {
        /// <summary>
        /// Rubrik
        /// </summary>
        [DataMember(Name="heading",EmitDefaultValue=false)]
        public string Heading { get; set; }
    
        /// <summary>
        /// Text
        /// </summary>
        [DataMember(Name="text",EmitDefaultValue=false)]
        public string Text { get; set; }
    
        /// <summary>
        /// Ordningsnummer
        /// </summary>
        [DataMember(Name="sortOrder",EmitDefaultValue=false)]
        public string SortOrder { get; set; }
    
    }
}