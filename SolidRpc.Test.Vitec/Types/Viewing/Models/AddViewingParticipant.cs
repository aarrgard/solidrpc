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
    
    }
}