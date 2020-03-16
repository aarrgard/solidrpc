using System.CodeDom.Compiler;
using System;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Viewing.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AddViewing {
        /// <summary>
        /// Starttid
        /// </summary>
        [DataMember(Name="startAt",EmitDefaultValue=false)]
        public DateTimeOffset? StartAt { get; set; }
    
        /// <summary>
        /// Sluttid
        /// </summary>
        [DataMember(Name="endsAt",EmitDefaultValue=false)]
        public DateTimeOffset? EndsAt { get; set; }
    
        /// <summary>
        /// Visas p&#229; internet
        /// </summary>
        [DataMember(Name="showOnInternet",EmitDefaultValue=false)]
        public bool? ShowOnInternet { get; set; }
    
        /// <summary>
        /// Bokningsbar p&#229; internet
        /// </summary>
        [DataMember(Name="bookableOnInternet",EmitDefaultValue=false)]
        public bool? BookableOnInternet { get; set; }
    
        /// <summary>
        /// Kommentar
        /// </summary>
        [DataMember(Name="comment",EmitDefaultValue=false)]
        public string Comment { get; set; }
    
    }
}