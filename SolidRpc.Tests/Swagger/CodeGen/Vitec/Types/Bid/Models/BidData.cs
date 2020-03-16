using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Bid.Models {
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
        /// Datum och tid f&#246;r budet
        /// </summary>
        [DataMember(Name="createdAt",EmitDefaultValue=false)]
        public System.DateTimeOffset? CreatedAt { get; set; }
    
        /// <summary>
        /// Villkor f&#246;r budet
        /// </summary>
        [DataMember(Name="condition",EmitDefaultValue=false)]
        public string Condition { get; set; }
    
        /// <summary>
        /// Anger om budet &#228;r dolt
        /// </summary>
        [DataMember(Name="isHidden",EmitDefaultValue=false)]
        public bool? IsHidden { get; set; }
    
    }
}