using System.CodeDom.Compiler;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Update.Contact;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Interest.Models {
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
        /// S&#246;kpreferenser
        /// </summary>
        [DataMember(Name="lookingForAccommodations",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Interest.Models.LookingForAccommodation> LookingForAccommodations { get; set; }
    
        /// <summary>
        /// Nuvarande boende
        /// </summary>
        [DataMember(Name="presentAccommodation",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Interest.Models.PresentAccommodation PresentAccommodation { get; set; }
    
        /// <summary>
        /// Notifiera handl&#228;ggaren p&#229; kontakten att ett intresse f&#246;r ett omr&#229;de inkommit, kr&#228;ver UserId f&#246;r nya kontakter
        /// </summary>
        [DataMember(Name="notifyUser",EmitDefaultValue=false)]
        public bool NotifyUser { get; set; }
    
    }
}