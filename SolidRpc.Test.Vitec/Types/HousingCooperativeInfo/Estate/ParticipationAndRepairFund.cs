using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.HousingCooperativeInfo.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ParticipationAndRepairFund {
        /// <summary>
        /// Reparationsfond
        /// </summary>
        [DataMember(Name="repairFund",EmitDefaultValue=false)]
        public double? RepairFund { get; set; }
    
        /// <summary>
        /// Andel av &#229;rsavgiften
        /// </summary>
        [DataMember(Name="participationOffAnnualFee",EmitDefaultValue=false)]
        public double? ParticipationOffAnnualFee { get; set; }
    
        /// <summary>
        /// Andel i f&#246;rening (%)
        /// </summary>
        [DataMember(Name="participationInAssociation",EmitDefaultValue=false)]
        public double? ParticipationInAssociation { get; set; }
    
        /// <summary>
        /// Kommentar till andelstal
        /// </summary>
        [DataMember(Name="shareComment",EmitDefaultValue=false)]
        public string ShareComment { get; set; }
    
    }
}