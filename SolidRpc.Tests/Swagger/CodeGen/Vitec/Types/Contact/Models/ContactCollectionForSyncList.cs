using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Contact.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class ContactCollectionForSyncList {
        /// <summary>
        /// Kund id
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Lista av personkontakter
        /// </summary>
        [DataMember(Name="persons",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Contact.Models.PersonForSyncList> Persons { get; set; }
    
        /// <summary>
        /// Lista av f&#246;retagskontakter
        /// </summary>
        [DataMember(Name="companies",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Contact.Models.CompanyForSyncList> Companies { get; set; }
    
        /// <summary>
        /// Lista av d&#246;dsbokontakter
        /// </summary>
        [DataMember(Name="estates",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Contact.Models.EstateForSyncList> Estates { get; set; }
    
    }
}