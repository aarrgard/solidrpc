using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.Common.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ProjectViewing {
        /// <summary>
        /// Objekt id
        /// </summary>
        [DataMember(Name="estateId",EmitDefaultValue=false)]
        public string EstateId { get; set; }
    
        /// <summary>
        /// Typ av objekt
        /// </summary>
        [DataMember(Name="estateType",EmitDefaultValue=false)]
        public string EstateType { get; set; }
    
        /// <summary>
        /// Visningsid
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Datum
        /// </summary>
        [DataMember(Name="date",EmitDefaultValue=false)]
        public DateTimeOffset? Date { get; set; }
    
        /// <summary>
        /// Fr&#229;n
        /// </summary>
        [DataMember(Name="startTime",EmitDefaultValue=false)]
        public DateTimeOffset? StartTime { get; set; }
    
        /// <summary>
        /// Till
        /// </summary>
        [DataMember(Name="endTime",EmitDefaultValue=false)]
        public DateTimeOffset? EndTime { get; set; }
    
        /// <summary>
        /// Kommentar
        /// </summary>
        [DataMember(Name="commentary",EmitDefaultValue=false)]
        public string Commentary { get; set; }
    
        /// <summary>
        /// Bokningsbar fr&#229;n Internet
        /// </summary>
        [DataMember(Name="bookableFromInternet",EmitDefaultValue=false)]
        public bool? BookableFromInternet { get; set; }
    
        /// <summary>
        /// Bokningsurl
        /// </summary>
        [DataMember(Name="bookingUrl",EmitDefaultValue=false)]
        public string BookingUrl { get; set; }
    
        /// <summary>
        /// Visa p&#229; Internet
        /// </summary>
        [DataMember(Name="showOnInternet",EmitDefaultValue=false)]
        public bool? ShowOnInternet { get; set; }
    
        /// <summary>
        /// Intressenter som varit p&#229; aktuell visning
        /// </summary>
        [DataMember(Name="participants",EmitDefaultValue=false)]
        public IEnumerable<string> Participants { get; set; }
    
    }
}