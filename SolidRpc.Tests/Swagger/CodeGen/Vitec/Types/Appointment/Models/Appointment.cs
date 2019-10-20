using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Appointment.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Appointment {
        /// <summary>
        /// �mne
        /// </summary>
        [DataMember(Name="subject",EmitDefaultValue=false)]
        public string Subject { get; set; }
    
        /// <summary>
        /// Plats
        /// </summary>
        [DataMember(Name="location",EmitDefaultValue=false)]
        public string Location { get; set; }
    
        /// <summary>
        /// Startdatum och tid
        /// </summary>
        [DataMember(Name="startsAt",EmitDefaultValue=false)]
        public DateTimeOffset StartsAt { get; set; }
    
        /// <summary>
        /// Slutdatum och tid
        /// </summary>
        [DataMember(Name="endsAt",EmitDefaultValue=false)]
        public DateTimeOffset EndsAt { get; set; }
    
        /// <summary>
        /// Hela dagen
        /// </summary>
        [DataMember(Name="allDayEvent",EmitDefaultValue=false)]
        public bool AllDayEvent { get; set; }
    
        /// <summary>
        /// Anteckning
        /// </summary>
        [DataMember(Name="note",EmitDefaultValue=false)]
        public string Note { get; set; }
    
        /// <summary>
        /// �terkommande
        /// </summary>
        [DataMember(Name="recurrence",EmitDefaultValue=false)]
        public bool Recurrence { get; set; }
    
        /// <summary>
        /// �terkommande per �r/m�nad/vecka/dag
        /// </summary>
        [DataMember(Name="recurrencePattern",EmitDefaultValue=false)]
        public string RecurrencePattern { get; set; }
    
        /// <summary>
        /// Dag/dagar i veckan (anv�nds f�r veckoliga m�ten)
        /// </summary>
        [DataMember(Name="recurrenceDayOfWeeks",EmitDefaultValue=false)]
        public IEnumerable<string> RecurrenceDayOfWeeks { get; set; }
    
        /// <summary>
        /// Interval
        /// </summary>
        [DataMember(Name="recurrenceInterval",EmitDefaultValue=false)]
        public int RecurrenceInterval { get; set; }
    
        /// <summary>
        /// M�naden (anv�nds f�r m�natliga m�ten)
        /// </summary>
        [DataMember(Name="recurrenceMonth",EmitDefaultValue=false)]
        public int RecurrenceMonth { get; set; }
    
        /// <summary>
        /// Dag i m�naden (anv�ns f�r �rliga och m�natliga m�ten)
        /// </summary>
        [DataMember(Name="recurrenceDay",EmitDefaultValue=false)]
        public int RecurrenceDay { get; set; }
    
    }
}