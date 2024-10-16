using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Tips.BusinessIntelligense;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class SentTip {
        /// <summary>
        /// Skapat
        /// </summary>
        [DataMember(Name="createdAt",EmitDefaultValue=false)]
        public DateTimeOffset? CreatedAt { get; set; }
    
        /// <summary>
        /// Avs&#228;ndare
        /// </summary>
        [DataMember(Name="sender",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.Sender Sender { get; set; }
    
        /// <summary>
        /// Mottagare
        /// </summary>
        [DataMember(Name="receiver",EmitDefaultValue=false)]
        public Receiver Receiver { get; set; }
    
        /// <summary>
        /// Lead
        /// </summary>
        [DataMember(Name="tip",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.Tip Tip { get; set; }
    
        /// <summary>
        /// Kontakt
        /// </summary>
        [DataMember(Name="contact",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.Contact Contact { get; set; }
    
        /// <summary>
        /// Bostad
        /// </summary>
        [DataMember(Name="estate",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.Estate Estate { get; set; }
    
    }
}