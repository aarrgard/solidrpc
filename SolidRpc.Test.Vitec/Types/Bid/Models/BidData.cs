using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Bid.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class BidData {
        /// <summary>
        /// Bud alias
        /// </summary>
        [DataMember(Name="alias",EmitDefaultValue=false)]
        public string Alias { get; set; }
    
        /// <summary>
        /// Budbelopp
        /// </summary>
        [DataMember(Name="amount",EmitDefaultValue=false)]
        public string Amount { get; set; }
    
        /// <summary>
        /// Datum och tid f�r budet
        /// </summary>
        [DataMember(Name="createdAt",EmitDefaultValue=false)]
        public DateTimeOffset CreatedAt { get; set; }
    
        /// <summary>
        /// Villkor f�r budet
        /// </summary>
        [DataMember(Name="condition",EmitDefaultValue=false)]
        public string Condition { get; set; }
    
        /// <summary>
        /// Anger om budet �r dolt
        /// </summary>
        [DataMember(Name="isHidden",EmitDefaultValue=false)]
        public bool IsHidden { get; set; }
    
    }
}