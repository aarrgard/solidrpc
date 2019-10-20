using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Bid.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Bid {
        /// <summary>
        /// Budets id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Bostadsid
        /// </summary>
        [DataMember(Name="estateId",EmitDefaultValue=false)]
        public string EstateId { get; set; }
    
        /// <summary>
        /// Kontaktid
        /// </summary>
        [DataMember(Name="contactId",EmitDefaultValue=false)]
        public string ContactId { get; set; }
    
        /// <summary>
        /// Om budet �r makulerat
        /// </summary>
        [DataMember(Name="isCanceled",EmitDefaultValue=false)]
        public bool IsCanceled { get; set; }
    
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