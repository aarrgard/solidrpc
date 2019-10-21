using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.PartnerService.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PersonTelephoneNumbers {
        /// <summary>
        /// Telefon bostad
        /// </summary>
        [DataMember(Name="home",EmitDefaultValue=false)]
        public string Home { get; set; }
    
        /// <summary>
        /// Telefon arbete
        /// </summary>
        [DataMember(Name="work",EmitDefaultValue=false)]
        public string Work { get; set; }
    
        /// <summary>
        /// Mobiltelefon
        /// </summary>
        [DataMember(Name="cell",EmitDefaultValue=false)]
        public string Cell { get; set; }
    
        /// <summary>
        /// &#214;vrig telefonnummer
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
    }
}