using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.FarmInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class FarmBuilding {
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
        /// Bj�lklag
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
        /// Utv pl�tarbete
        /// </summary>
        [DataMember(Name="externallyMetalWork",EmitDefaultValue=false)]
        public string ExternallyMetalWork { get; set; }
    
        /// <summary>
        /// F�nster
        /// </summary>
        [DataMember(Name="windows",EmitDefaultValue=false)]
        public string Windows { get; set; }
    
        /// <summary>
        /// Uppv�rmning
        /// </summary>
        [DataMember(Name="heating",EmitDefaultValue=false)]
        public string Heating { get; set; }
    
        /// <summary>
        /// Vatten/avlopp
        /// </summary>
        [DataMember(Name="waterAndDrain",EmitDefaultValue=false)]
        public string WaterAndDrain { get; set; }
    
        /// <summary>
        /// �vrigt
        /// </summary>
        [DataMember(Name="others",EmitDefaultValue=false)]
        public string Others { get; set; }
    
    }
}