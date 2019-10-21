using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EstateAgentOfficeAffiliation {
        /// <summary>
        /// Telefon arbete f&#246;r aktuellt kontor
        /// </summary>
        [DataMember(Name="workPhone",EmitDefaultValue=false)]
        public string WorkPhone { get; set; }
    
        /// <summary>
        /// Kontorsid
        /// </summary>
        [DataMember(Name="officeId",EmitDefaultValue=false)]
        public string OfficeId { get; set; }
    
        /// <summary>
        /// Kundid
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
    }
}