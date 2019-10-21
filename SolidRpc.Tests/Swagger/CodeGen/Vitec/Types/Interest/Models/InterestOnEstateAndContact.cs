using System.CodeDom.Compiler;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Update.Contact;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Interest.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class InterestOnEstateAndContact {
        /// <summary>
        /// Person
        /// </summary>
        [DataMember(Name="updatePerson",EmitDefaultValue=false)]
        public UpdatePerson UpdatePerson { get; set; }
    
        /// <summary>
        /// Objektid
        /// </summary>
        [DataMember(Name="estateId",EmitDefaultValue=false)]
        public string EstateId { get; set; }
    
        /// <summary>
        /// Nuvarande boende
        /// </summary>
        [DataMember(Name="presentAccommodation",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Interest.Models.PresentAccommodation PresentAccommodation { get; set; }
    
        /// <summary>
        /// Intresseanteckning.
        /// </summary>
        [DataMember(Name="interestNote",EmitDefaultValue=false)]
        public string InterestNote { get; set; }
    
        /// <summary>
        /// Notifiera handl&#228;ggaren p&#229; bostaden att intresseanm&#228;lan inkommit
        /// </summary>
        [DataMember(Name="notifyUser",EmitDefaultValue=false)]
        public bool NotifyUser { get; set; }
    
    }
}