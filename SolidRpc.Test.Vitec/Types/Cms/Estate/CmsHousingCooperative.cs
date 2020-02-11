using System.CodeDom.Compiler;
using SolidRpc.Test.Vitec.Types.HousingCooperative.Cms;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Common.Cms;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.Cms.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsHousingCooperative {
        /// <summary>
        /// Basinformation
        /// </summary>
        [DataMember(Name="baseInformation",EmitDefaultValue=false)]
        public CmsHousingCooperativBaseInformation BaseInformation { get; set; }
    
        /// <summary>
        /// Interi&#246;r
        /// </summary>
        [DataMember(Name="interior",EmitDefaultValue=false)]
        public CmsHousingCooperativInterior Interior { get; set; }
    
        /// <summary>
        /// Balkong/uteplats
        /// </summary>
        [DataMember(Name="balconyPatio",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Cms.Estate.BalconyPatio BalconyPatio { get; set; }
    
        /// <summary>
        /// V&#229;ning/hiss
        /// </summary>
        [DataMember(Name="floorAndElevator",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Cms.Estate.FloorAndElevator FloorAndElevator { get; set; }
    
        /// <summary>
        /// Pris information
        /// </summary>
        [DataMember(Name="price",EmitDefaultValue=false)]
        public CmsPrice Price { get; set; }
    
        /// <summary>
        /// Lista &#246;ver s&#228;ljarid&#39;n
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