using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Hits.BusinessIntelligense {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EstateHit {
        /// <summary>
        /// ObjektId
        /// </summary>
        [DataMember(Name="estateId",EmitDefaultValue=false)]
        public string EstateId { get; set; }
    
        /// <summary>
        /// Tidpunkt f&#246;r bes&#246;k
        /// </summary>
        [DataMember(Name="occurredAt",EmitDefaultValue=false)]
        public DateTimeOffset? OccurredAt { get; set; }
    
        /// <summary>
        /// Bes&#246;kets l&#228;ngd
        /// </summary>
        [DataMember(Name="duration",EmitDefaultValue=false)]
        public int? Duration { get; set; }
    
        /// <summary>
        /// Bes&#246;karens user agent (browser)
        /// </summary>
        [DataMember(Name="userAgent",EmitDefaultValue=false)]
        public string UserAgent { get; set; }
    
        /// <summary>
        /// Vart kom bes&#246;ket ifr&#229;n
        /// </summary>
        [DataMember(Name="referer",EmitDefaultValue=false)]
        public string Referer { get; set; }
    
    }
}