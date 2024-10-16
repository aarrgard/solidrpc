using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Interest.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class InterestOnEstate {
        /// <summary>
        /// KundId
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Objektid
        /// </summary>
        [DataMember(Name="estateId",EmitDefaultValue=false)]
        public string EstateId { get; set; }
    
        /// <summary>
        /// Kontaktid
        /// </summary>
        [DataMember(Name="contactId",EmitDefaultValue=false)]
        public string ContactId { get; set; }
    
        /// <summary>
        /// Anteckning
        /// </summary>
        [DataMember(Name="note",EmitDefaultValue=false)]
        public string Note { get; set; }
    
        /// <summary>
        /// Intresseanteckning.
        /// </summary>
        [DataMember(Name="interestNote",EmitDefaultValue=false)]
        public string InterestNote { get; set; }
    
        /// <summary>
        /// Nuvarande boende
        /// </summary>
        [DataMember(Name="presentAccommodation",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Interest.Models.PresentAccommodation PresentAccommodation { get; set; }
    
        /// <summary>
        /// En uppgift som ska l&#228;ggas till.
        /// </summary>
        [DataMember(Name="task",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Task.Models.Task Task { get; set; }
    
        /// <summary>
        /// Notifiera handl&#228;ggaren p&#229; bostaden att intresseanm&#228;lan inkommit
        /// </summary>
        [DataMember(Name="notifyUser",EmitDefaultValue=false)]
        public bool? NotifyUser { get; set; }
    
    }
}