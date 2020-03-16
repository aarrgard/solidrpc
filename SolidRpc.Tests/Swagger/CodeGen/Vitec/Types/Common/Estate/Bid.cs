using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Bid {
        /// <summary>
        /// Budid
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Alias
        /// </summary>
        [DataMember(Name="alias",EmitDefaultValue=false)]
        public string Alias { get; set; }
    
        /// <summary>
        /// Bud
        /// </summary>
        [DataMember(Name="amount",EmitDefaultValue=false)]
        public double? Amount { get; set; }
    
        /// <summary>
        /// Datum och tid
        /// </summary>
        [DataMember(Name="dateAndTime",EmitDefaultValue=false)]
        public System.DateTimeOffset? DateAndTime { get; set; }
    
        /// <summary>
        /// Annulerat (Ja eller nej)
        /// </summary>
        [DataMember(Name="cancelled",EmitDefaultValue=false)]
        public bool? Cancelled { get; set; }
    
        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name="status",EmitDefaultValue=false)]
        public string Status { get; set; }
    
    }
}