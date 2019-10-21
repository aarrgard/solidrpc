using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Contact.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ContactCollection {
        /// <summary>
        /// Kundid
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Lista av personkontakter
        /// </summary>
        [DataMember(Name="persons",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Contact.Models.Person> Persons { get; set; }
    
        /// <summary>
        /// Lista av d&#246;dsbon
        /// </summary>
        [DataMember(Name="estates",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Contact.Models.Estate> Estates { get; set; }
    
        /// <summary>
        /// Lista av f&#246;retagskontakter
        /// </summary>
        [DataMember(Name="companies",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Contact.Models.Company> Companies { get; set; }
    
    }
}