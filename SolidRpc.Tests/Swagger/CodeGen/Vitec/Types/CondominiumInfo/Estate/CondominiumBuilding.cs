using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CondominiumInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CondominiumBuilding {
        /// <summary>
        /// Bygg&#229;r
        /// </summary>
        [DataMember(Name="buildingYear",EmitDefaultValue=false)]
        public string BuildingYear { get; set; }
    
        /// <summary>
        /// Bygg&#229;r kommentar
        /// </summary>
        [DataMember(Name="buildingYearComment",EmitDefaultValue=false)]
        public string BuildingYearComment { get; set; }
    
        /// <summary>
        /// F&#246;nster
        /// </summary>
        [DataMember(Name="window",EmitDefaultValue=false)]
        public string Window { get; set; }
    
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
        /// Renoveringar
        /// </summary>
        [DataMember(Name="renovations",EmitDefaultValue=false)]
        public string Renovations { get; set; }
    
        /// <summary>
        /// &#214;vrigt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
    }
}