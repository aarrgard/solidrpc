using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Viewing.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Viewing {
        /// <summary>
        /// Id p&#229; visningen
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Starttid
        /// </summary>
        [DataMember(Name="startsAt",EmitDefaultValue=false)]
        public DateTimeOffset StartsAt { get; set; }
    
        /// <summary>
        /// Sluttid
        /// </summary>
        [DataMember(Name="endsAt",EmitDefaultValue=false)]
        public DateTimeOffset EndsAt { get; set; }
    
        /// <summary>
        /// Visas p&#229; internet
        /// </summary>
        [DataMember(Name="showOnInternet",EmitDefaultValue=false)]
        public bool ShowOnInternet { get; set; }
    
        /// <summary>
        /// Kommentar
        /// </summary>
        [DataMember(Name="commentary",EmitDefaultValue=false)]
        public string Commentary { get; set; }
    
        /// <summary>
        /// Max antal deltagare per visningtillf&#228;lle
        /// </summary>
        [DataMember(Name="participantPerTimeSlot",EmitDefaultValue=false)]
        public int ParticipantPerTimeSlot { get; set; }
    
        /// <summary>
        /// Bokningsbar p&#229; internet
        /// </summary>
        [DataMember(Name="bookableFromInternet",EmitDefaultValue=false)]
        public bool BookableFromInternet { get; set; }
    
        /// <summary>
        /// Bokningsbar p&#229; internet fram till
        /// </summary>
        [DataMember(Name="noLaterThen",EmitDefaultValue=false)]
        public DateTimeOffset NoLaterThen { get; set; }
    
        /// <summary>
        /// URL till till Bookning
        /// </summary>
        [DataMember(Name="bookingUrl",EmitDefaultValue=false)]
        public string BookingUrl { get; set; }
    
        /// <summary>
        /// Visningstillf&#228;llen
        /// </summary>
        [DataMember(Name="timeSlots",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Viewing.Models.TimeSlot> TimeSlots { get; set; }
    
        /// <summary>
        /// Id p&#229; objektet
        /// </summary>
        [DataMember(Name="estateId",EmitDefaultValue=false)]
        public string EstateId { get; set; }
    
    }
}