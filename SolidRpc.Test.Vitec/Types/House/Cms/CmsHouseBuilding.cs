using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.House.Cms {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsHouseBuilding {
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
        /// Bj&#228;lklag
        /// </summary>
        [DataMember(Name="beam",EmitDefaultValue=false)]
        public string Beam { get; set; }
    
        /// <summary>
        /// Tak
        /// </summary>
        [DataMember(Name="roof",EmitDefaultValue=false)]
        public string Roof { get; set; }
    
        /// <summary>
        /// Grundl&#228;ggning
        /// </summary>
        [DataMember(Name="foundation",EmitDefaultValue=false)]
        public string Foundation { get; set; }
    
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