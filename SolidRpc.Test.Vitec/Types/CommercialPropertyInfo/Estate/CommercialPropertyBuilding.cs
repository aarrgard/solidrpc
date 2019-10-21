using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Test.Vitec.Types.Common.Estate;
namespace SolidRpc.Test.Vitec.Types.CommercialPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CommercialPropertyBuilding {
        /// <summary>
        /// Grundmur
        /// </summary>
        [DataMember(Name="foundationWall",EmitDefaultValue=false)]
        public string FoundationWall { get; set; }
    
        /// <summary>
        /// Grund
        /// </summary>
        [DataMember(Name="foundation",EmitDefaultValue=false)]
        public string Foundation { get; set; }
    
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
        /// Fasad
        /// </summary>
        [DataMember(Name="facade",EmitDefaultValue=false)]
        public string Facade { get; set; }
    
        /// <summary>
        /// Tak
        /// </summary>
        [DataMember(Name="roof",EmitDefaultValue=false)]
        public string Roof { get; set; }
    
        /// <summary>
        /// Utv pl&#229;tarbete
        /// </summary>
        [DataMember(Name="externallyMetalWork",EmitDefaultValue=false)]
        public string ExternallyMetalWork { get; set; }
    
        /// <summary>
        /// F&#246;nster
        /// </summary>
        [DataMember(Name="windows",EmitDefaultValue=false)]
        public string Windows { get; set; }
    
        /// <summary>
        /// Ventilation
        /// </summary>
        [DataMember(Name="ventilation",EmitDefaultValue=false)]
        public Ventilation Ventilation { get; set; }
    
        /// <summary>
        /// Kyla
        /// </summary>
        [DataMember(Name="cold",EmitDefaultValue=false)]
        public string Cold { get; set; }
    
        /// <summary>
        /// Kyla, kommentar
        /// </summary>
        [DataMember(Name="coldComment",EmitDefaultValue=false)]
        public string ColdComment { get; set; }
    
        /// <summary>
        /// Styr/&#214;vervakning
        /// </summary>
        [DataMember(Name="controlMonitoring",EmitDefaultValue=false)]
        public string ControlMonitoring { get; set; }
    
        /// <summary>
        /// Vatten/avlopp
        /// </summary>
        [DataMember(Name="waterAndDrain",EmitDefaultValue=false)]
        public string WaterAndDrain { get; set; }
    
        /// <summary>
        /// Uppv&#228;rmning
        /// </summary>
        [DataMember(Name="heating",EmitDefaultValue=false)]
        public string Heating { get; set; }
    
        /// <summary>
        /// &#214;vrigt
        /// </summary>
        [DataMember(Name="others",EmitDefaultValue=false)]
        public string Others { get; set; }
    
    }
}