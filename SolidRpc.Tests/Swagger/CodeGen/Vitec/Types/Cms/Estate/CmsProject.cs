using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Cms.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsProject {
        /// <summary>
        /// Grundinformation om projektet
        /// </summary>
        [DataMember(Name="baseInformation",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Cms.Estate.CmsProjectBaseInformation BaseInformation { get; set; }
    
        /// <summary>
        /// S&#228;ljande rubrik p&#229; projektet
        /// </summary>
        [DataMember(Name="sellingHeading",EmitDefaultValue=false)]
        public string SellingHeading { get; set; }
    
        /// <summary>
        /// L&#229;ng beskrivning av projektet
        /// </summary>
        [DataMember(Name="longSellingDescription",EmitDefaultValue=false)]
        public string LongSellingDescription { get; set; }
    
    }
}