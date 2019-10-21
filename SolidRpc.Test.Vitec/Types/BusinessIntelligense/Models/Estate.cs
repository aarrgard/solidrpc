using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Estate {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Bostadsstatus
        /// </summary>
        [DataMember(Name="estateStatus",EmitDefaultValue=false)]
        public string EstateStatus { get; set; }
    
        /// <summary>
        /// Status i beslutst&#246;dsrapporter
        /// </summary>
        [DataMember(Name="status",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.BusinessIntelligense.Models.Status Status { get; set; }
    
    }
}