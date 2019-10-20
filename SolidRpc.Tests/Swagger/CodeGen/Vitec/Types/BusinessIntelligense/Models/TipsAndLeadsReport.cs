using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class TipsAndLeadsReport {
        /// <summary>
        /// Inkommande leads
        /// </summary>
        [DataMember(Name="incomingLeads",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.IncomingLead> IncomingLeads { get; set; }
    
        /// <summary>
        /// Utg�ende leads
        /// </summary>
        [DataMember(Name="outgoingLeads",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.OutgoingLead> OutgoingLeads { get; set; }
    
        /// <summary>
        /// Tips mellan kollegor
        /// </summary>
        [DataMember(Name="tips",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.SentTip> Tips { get; set; }
    
    }
}