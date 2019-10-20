using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Estate.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PremisesBuilding {
        /// <summary>
        /// Bygg�r
        /// </summary>
        [DataMember(Name="constructionYear",EmitDefaultValue=false)]
        public string ConstructionYear { get; set; }
    
        /// <summary>
        /// Renoverings�r
        /// </summary>
        [DataMember(Name="renovationYear",EmitDefaultValue=false)]
        public string RenovationYear { get; set; }
    
        /// <summary>
        /// Antal rum
        /// </summary>
        [DataMember(Name="numberOfRooms",EmitDefaultValue=false)]
        public double NumberOfRooms { get; set; }
    
        /// <summary>
        /// Uppv�rmning
        /// </summary>
        [DataMember(Name="heating",EmitDefaultValue=false)]
        public string Heating { get; set; }
    
        /// <summary>
        /// Ventilation
        /// </summary>
        [DataMember(Name="ventilation",EmitDefaultValue=false)]
        public string Ventilation { get; set; }
    
        /// <summary>
        /// Renoveringar
        /// </summary>
        [DataMember(Name="renovation",EmitDefaultValue=false)]
        public string Renovation { get; set; }
    
        /// <summary>
        /// Planl�sning
        /// </summary>
        [DataMember(Name="plan",EmitDefaultValue=false)]
        public string Plan { get; set; }
    
        /// <summary>
        /// �vrigt
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
    }
}