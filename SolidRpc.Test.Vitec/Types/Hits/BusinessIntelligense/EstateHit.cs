using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Hits.BusinessIntelligense {
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
        /// Tidpunkt f�r bes�k
        /// </summary>
        [DataMember(Name="occurredAt",EmitDefaultValue=false)]
        public DateTimeOffset OccurredAt { get; set; }
    
        /// <summary>
        /// Bes�kets l�ngd
        /// </summary>
        [DataMember(Name="duration",EmitDefaultValue=false)]
        public int Duration { get; set; }
    
        /// <summary>
        /// Bes�karens user agent (browser)
        /// </summary>
        [DataMember(Name="userAgent",EmitDefaultValue=false)]
        public string UserAgent { get; set; }
    
        /// <summary>
        /// Vart kom bes�ket ifr�n
        /// </summary>
        [DataMember(Name="referer",EmitDefaultValue=false)]
        public string Referer { get; set; }
    
    }
}