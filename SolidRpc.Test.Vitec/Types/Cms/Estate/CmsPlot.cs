using System.CodeDom.Compiler;
using SolidRpc.Test.Vitec.Types.Plot.Cms;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Common.Cms;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.Cms.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsPlot {
        /// <summary>
        /// Basinformation
        /// </summary>
        [DataMember(Name="baseInformation",EmitDefaultValue=false)]
        public CmsPlotBaseInformation BaseInformation { get; set; }
    
        /// <summary>
        /// Tomt
        /// </summary>
        [DataMember(Name="plot",EmitDefaultValue=false)]
        public CmsPlotInformation Plot { get; set; }
    
        /// <summary>
        /// Pris information
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public CmsPrice Price { get; set; }
    
        /// <summary>
        /// Lista &#246;ver s&#228;ljar id&#39;n
        /// </summary>
        [DataMember(Name="sellers",EmitDefaultValue=false)]
        public IEnumerable<string> Sellers { get; set; }
    
        /// <summary>
        /// Lista &#246;ver intressenters id&#39;n
        /// </summary>
        [DataMember(Name="interests",EmitDefaultValue=false)]
        public IEnumerable<string> Interests { get; set; }
    
        /// <summary>
        /// Beskrivning
        /// </summary>
        [DataMember(Name="description",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Cms.Estate.Description Description { get; set; }
    
        /// <summary>
        /// Anv&#228;ndarid
        /// </summary>
        [DataMember(Name="userId",EmitDefaultValue=false)]
        public string UserId { get; set; }
    
    }
}