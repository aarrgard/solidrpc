using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Office.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PublicOfficeCritera {
        /// <summary>
        /// Unik identifierare p&#229; butiken
        /// </summary>
        [DataMember(Name="officeId",EmitDefaultValue=false)]
        public string OfficeId { get; set; }
    
        /// <summary>
        /// Kontorsid
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
    }
}