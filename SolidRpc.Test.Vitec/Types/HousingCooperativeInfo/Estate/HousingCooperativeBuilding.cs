using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.HousingCooperativeInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class HousingCooperativeBuilding {
        /// <summary>
        /// Typ av byggnad
        /// </summary>
        [DataMember(Name="buildingType",EmitDefaultValue=false)]
        public string BuildingType { get; set; }
    
        /// <summary>
        /// Byggnations&#229;r
        /// </summary>
        [DataMember(Name="buildingYear",EmitDefaultValue=false)]
        public string BuildingYear { get; set; }
    
        /// <summary>
        /// Kommentar till byggnads&#229;r
        /// </summary>
        [DataMember(Name="commentaryForBuildingYear",EmitDefaultValue=false)]
        public string CommentaryForBuildingYear { get; set; }
    
        /// <summary>
        /// Uppv&#228;rmning
        /// </summary>
        [DataMember(Name="heating",EmitDefaultValue=false)]
        public string Heating { get; set; }
    
        /// <summary>
        /// F&#246;nster
        /// </summary>
        [DataMember(Name="windows",EmitDefaultValue=false)]
        public string Windows { get; set; }
    
        /// <summary>
        /// &#214;vrigt om byggnaden
        /// </summary>
        [DataMember(Name="otherAboutTheBuildning",EmitDefaultValue=false)]
        public string OtherAboutTheBuildning { get; set; }
    
        /// <summary>
        /// &#214;vrigta byggnader
        /// </summary>
        [DataMember(Name="otherBuildings",EmitDefaultValue=false)]
        public string OtherBuildings { get; set; }
    
        /// <summary>
        /// Fastighetsbeteckning
        /// </summary>
        [DataMember(Name="propertyUnitDesignation",EmitDefaultValue=false)]
        public string PropertyUnitDesignation { get; set; }
    
        /// <summary>
        /// Renoveringar
        /// </summary>
        [DataMember(Name="renovations",EmitDefaultValue=false)]
        public string Renovations { get; set; }
    
    }
}