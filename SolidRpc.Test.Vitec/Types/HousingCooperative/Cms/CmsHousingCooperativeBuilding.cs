using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.HousingCooperative.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsHousingCooperativeBuilding {
        /// <summary>
        /// Byggnadstyp
        /// </summary>
        [DataMember(Name="buildingType",EmitDefaultValue=false)]
        public string BuildingType { get; set; }
    
        /// <summary>
        /// Byggnations&#229;r
        /// </summary>
        [DataMember(Name="buildingYear",EmitDefaultValue=false)]
        public string BuildingYear { get; set; }
    
        /// <summary>
        /// F&#246;nster
        /// </summary>
        [DataMember(Name="windows",EmitDefaultValue=false)]
        public string Windows { get; set; }
    
        /// <summary>
        /// Uppv&#228;rmning
        /// </summary>
        [DataMember(Name="heating",EmitDefaultValue=false)]
        public string Heating { get; set; }
    
        /// <summary>
        /// &#214;vrigt om byggnaden
        /// </summary>
        [DataMember(Name="otherAboutTheBuildning",EmitDefaultValue=false)]
        public string OtherAboutTheBuildning { get; set; }
    
        /// <summary>
        /// &#214;vriga byggnader
        /// </summary>
        [DataMember(Name="otherBuildings",EmitDefaultValue=false)]
        public string OtherBuildings { get; set; }
    
    }
}