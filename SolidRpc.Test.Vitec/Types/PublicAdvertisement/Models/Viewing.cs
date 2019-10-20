using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Viewing {
        /// <summary>
        /// Visningsid
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// N�r visningen startar
        /// </summary>
        [DataMember(Name="startsAt",EmitDefaultValue=false)]
        public DateTimeOffset StartsAt { get; set; }
    
        /// <summary>
        /// N�r visningen slutar
        /// </summary>
        [DataMember(Name="endsAt",EmitDefaultValue=false)]
        public DateTimeOffset EndsAt { get; set; }
    
        /// <summary>
        /// Kommentar
        /// </summary>
        [DataMember(Name="comment",EmitDefaultValue=false)]
        public string Comment { get; set; }
    
    }
}