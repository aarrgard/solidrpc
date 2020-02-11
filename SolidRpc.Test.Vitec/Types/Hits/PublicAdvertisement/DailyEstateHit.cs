using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Hits.PublicAdvertisement {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class DailyEstateHit {
        /// <summary>
        /// ObjektId
        /// </summary>
        [DataMember(Name="estateId",EmitDefaultValue=false)]
        public string EstateId { get; set; }
    
        /// <summary>
        /// Dag f&#246;r bes&#246;ket. (ev. tidangivelse ignoreras)
        /// </summary>
        [DataMember(Name="date",EmitDefaultValue=false)]
        public DateTimeOffset Date { get; set; }
    
        /// <summary>
        /// Antal bes&#246;k.
        /// </summary>
        [DataMember(Name="count",EmitDefaultValue=false)]
        public int Count { get; set; }
    
    }
}