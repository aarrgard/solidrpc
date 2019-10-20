using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class OutgoingLead {
        /// <summary>
        /// Skapat
        /// </summary>
        [DataMember(Name="createdAt",EmitDefaultValue=false)]
        public DateTimeOffset CreatedAt { get; set; }
    
        /// <summary>
        /// Anv�ndare
        /// </summary>
        [DataMember(Name="user",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.User User { get; set; }
    
        /// <summary>
        /// Avs�ndare
        /// </summary>
        [DataMember(Name="sender",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.Sender Sender { get; set; }
    
        /// <summary>
        /// Mottagare
        /// </summary>
        [DataMember(Name="receiver",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.Receiver Receiver { get; set; }
    
        /// <summary>
        /// Lead
        /// </summary>
        [DataMember(Name="lead",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Outgoing.BusinessIntelligense.Lead Lead { get; set; }
    
        /// <summary>
        /// Kontakt
        /// </summary>
        [DataMember(Name="contact",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.Contact Contact { get; set; }
    
    }
}