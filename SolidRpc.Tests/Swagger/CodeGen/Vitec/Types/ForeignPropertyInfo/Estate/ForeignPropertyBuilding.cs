using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ForeignPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ForeignPropertyBuilding {
        /// <summary>
        /// Typ av byggnad
        /// </summary>
        [DataMember(Name="buildingType",EmitDefaultValue=false)]
        public string BuildingType { get; set; }
    
        /// <summary>
        /// Byggnations�r
        /// </summary>
        [DataMember(Name="buildingYear",EmitDefaultValue=false)]
        public string BuildingYear { get; set; }
    
        /// <summary>
        /// Kommentar till byggnads�r
        /// </summary>
        [DataMember(Name="commentaryForBuildingYear",EmitDefaultValue=false)]
        public string CommentaryForBuildingYear { get; set; }
    
        /// <summary>
        /// Fasad
        /// </summary>
        [DataMember(Name="facade",EmitDefaultValue=false)]
        public string Facade { get; set; }
    
        /// <summary>
        /// Stomme
        /// </summary>
        [DataMember(Name="frame",EmitDefaultValue=false)]
        public string Frame { get; set; }
    
        /// <summary>
        /// F�nster
        /// </summary>
        [DataMember(Name="windows",EmitDefaultValue=false)]
        public string Windows { get; set; }
    
        /// <summary>
        /// Bj�lklag
        /// </summary>
        [DataMember(Name="beam",EmitDefaultValue=false)]
        public string Beam { get; set; }
    
        /// <summary>
        /// Utv�ndigt pl�tarbete
        /// </summary>
        [DataMember(Name="externallySheetMetalWork",EmitDefaultValue=false)]
        public string ExternallySheetMetalWork { get; set; }
    
        /// <summary>
        /// Grundmur
        /// </summary>
        [DataMember(Name="foundationWall",EmitDefaultValue=false)]
        public string FoundationWall { get; set; }
    
        /// <summary>
        /// Tak
        /// </summary>
        [DataMember(Name="roof",EmitDefaultValue=false)]
        public string Roof { get; set; }
    
        /// <summary>
        /// Grundl�ggning
        /// </summary>
        [DataMember(Name="foundation",EmitDefaultValue=false)]
        public string Foundation { get; set; }
    
        /// <summary>
        /// Uppv�rmning
        /// </summary>
        [DataMember(Name="heating",EmitDefaultValue=false)]
        public string Heating { get; set; }
    
        /// <summary>
        /// �vrigt om byggnaden
        /// </summary>
        [DataMember(Name="otherAboutTheBuildning",EmitDefaultValue=false)]
        public string OtherAboutTheBuildning { get; set; }
    
        /// <summary>
        /// �vriga byggnader
        /// </summary>
        [DataMember(Name="otherBuildings",EmitDefaultValue=false)]
        public string OtherBuildings { get; set; }
    
        /// <summary>
        /// Renoveringar
        /// </summary>
        [DataMember(Name="renovations",EmitDefaultValue=false)]
        public string Renovations { get; set; }
    
    }
}