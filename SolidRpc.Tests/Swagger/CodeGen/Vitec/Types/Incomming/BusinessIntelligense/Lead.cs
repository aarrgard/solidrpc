using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Incomming.BusinessIntelligense {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Lead {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Namn p� lead
        /// </summary>
        [DataMember(Name="title",EmitDefaultValue=false)]
        public string Title { get; set; }
    
        /// <summary>
        /// Typ av lead
        /// </summary>
        [DataMember(Name="type",EmitDefaultValue=false)]
        public string Type { get; set; }
    
        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name="state",EmitDefaultValue=false)]
        public string State { get; set; }
    
        /// <summary>
        /// Meddelande
        /// </summary>
        [DataMember(Name="message",EmitDefaultValue=false)]
        public string Message { get; set; }
    
    }
}