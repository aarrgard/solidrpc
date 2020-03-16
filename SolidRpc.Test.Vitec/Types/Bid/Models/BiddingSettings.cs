using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Bid.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class BiddingSettings {
        /// <summary>
        /// Hur bud ska visas p&#229; internet
        /// </summary>
        [DataMember(Name="biddingOptions",EmitDefaultValue=false)]
        public string BiddingOptions { get; set; }
    
        /// <summary>
        /// Sms budgivning
        /// </summary>
        [DataMember(Name="smsBidding",EmitDefaultValue=false)]
        public bool? SmsBidding { get; set; }
    
    }
}