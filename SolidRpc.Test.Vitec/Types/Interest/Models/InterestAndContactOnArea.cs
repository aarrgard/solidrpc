using System.CodeDom.Compiler;
using SolidRpc.Test.Vitec.Types.Update.Contact;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.Interest.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class InterestAndContactOnArea {
        /// <summary>
        /// Person
        /// </summary>
        [DataMember(Name="updatePerson",EmitDefaultValue=false)]
        public UpdatePerson UpdatePerson { get; set; }
    
        /// <summary>
        /// S�kpreferenser
        /// </summary>
        [DataMember(Name="lookingForAccommodations",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Interest.Models.LookingForAccommodation> LookingForAccommodations { get; set; }
    
        /// <summary>
        /// Nuvarande boende
        /// </summary>
        [DataMember(Name="presentAccommodation",EmitDefaultValue=false)]
        public SolidRpc.Test.Vitec.Types.Interest.Models.PresentAccommodation PresentAccommodation { get; set; }
    
        /// <summary>
        /// Notifiera handl�ggaren p� kontakten att ett intresse f�r ett omr�de inkommit, kr�ver UserId f�r nya kontakter
        /// </summary>
        [DataMember(Name="notifyUser",EmitDefaultValue=false)]
        public bool NotifyUser { get; set; }
    
    }
}