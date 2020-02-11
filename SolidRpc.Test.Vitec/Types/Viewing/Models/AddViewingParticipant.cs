using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Viewing.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class AddViewingParticipant {
        /// <summary>
        /// Deltog
        /// </summary>
        [DataMember(Name="participated",EmitDefaultValue=false)]
        public bool Participated { get; set; }
    
        /// <summary>
        /// Minuter innan visningen som p&#229;minnelse ska skickas ut
        /// </summary>
        [DataMember(Name="reminderTime",EmitDefaultValue=false)]
        public int ReminderTime { get; set; }
    
        /// <summary>
        /// SMS p&#229;minnelse
        /// </summary>
        [DataMember(Name="smsReminder",EmitDefaultValue=false)]
        public bool SmsReminder { get; set; }
    
        /// <summary>
        /// Bekr&#228;ftelse via epost
        /// </summary>
        [DataMember(Name="mailConfirmation",EmitDefaultValue=false)]
        public bool MailConfirmation { get; set; }
    
        /// <summary>
        /// Bekr&#228;ftelse via SMS
        /// </summary>
        [DataMember(Name="smsConfirmation",EmitDefaultValue=false)]
        public bool SmsConfirmation { get; set; }
    
        /// <summary>
        /// Meddelande fr&#229;n kontaktpersonen. Meddelandet kommer att sparas p&#229; det lead som skapas i samband med att visningsdeltagaren l&#228;ggs till.
        /// </summary>
        [DataMember(Name="contactMessage",EmitDefaultValue=false)]
        public string ContactMessage { get; set; }
    
        /// <summary>
        /// Leadsk&#228;lla - Det lead som skapas i samband med anm&#228;lan till visning kommer att kopplas till aktuell leadsk&#228;lla.
        /// Leadsk&#228;llan f&#229;r inte ha n&#229;got definierat arbetsfl&#246;de, dvs det skall bara inneh&#229;lla information.
        /// Om f&#228;ltet inte anges, kommer leadet att kopplas till en f&#246;rvald leadsk&#228;lla.
        /// </summary>
        [DataMember(Name="leadSourceId",EmitDefaultValue=false)]
        public string LeadSourceId { get; set; }
    
    }
}