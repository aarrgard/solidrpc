using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class TelephoneNumbers {
        /// <summary>
        /// Telefon arbete
        /// </summary>
        [DataMember(Name="work",EmitDefaultValue=false)]
        public string Work { get; set; }
    
        /// <summary>
        /// Telefon mobil
        /// </summary>
        [DataMember(Name="cell",EmitDefaultValue=false)]
        public string Cell { get; set; }
    
    }
}