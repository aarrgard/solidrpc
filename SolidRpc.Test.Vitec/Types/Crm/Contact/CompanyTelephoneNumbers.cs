using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Crm.Contact {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CompanyTelephoneNumbers {
        /// <summary>
        /// Telefon v�xel
        /// </summary>
        [DataMember(Name="switch",EmitDefaultValue=false)]
        public string Switch { get; set; }
    
        /// <summary>
        /// �vrig telefonnummer
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
    }
}