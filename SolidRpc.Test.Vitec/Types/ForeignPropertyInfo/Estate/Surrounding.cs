using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.ForeignPropertyInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Surrounding {
        /// <summary>
        /// Spr�kkod se:https://sv.wikipedia.org/wiki/ISO_639
        /// </summary>
        [DataMember(Name="language",EmitDefaultValue=false)]
        public string Language { get; set; }
    
        /// <summary>
        /// Allm�nt om omr�det
        /// </summary>
        [DataMember(Name="generalAboutArea",EmitDefaultValue=false)]
        public string GeneralAboutArea { get; set; }
    
        /// <summary>
        /// N�rservice
        /// </summary>
        [DataMember(Name="nearService",EmitDefaultValue=false)]
        public string NearService { get; set; }
    
        /// <summary>
        /// Kommunikation
        /// </summary>
        [DataMember(Name="communication",EmitDefaultValue=false)]
        public string Communication { get; set; }
    
        /// <summary>
        /// Parkering
        /// </summary>
        [DataMember(Name="parking",EmitDefaultValue=false)]
        public string Parking { get; set; }
    
        /// <summary>
        /// �vrigt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
    }
}