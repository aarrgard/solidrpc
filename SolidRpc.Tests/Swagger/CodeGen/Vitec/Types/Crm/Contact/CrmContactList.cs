using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Crm.Contact {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CrmContactList {
        /// <summary>
        /// Personer som matchats
        /// </summary>
        [DataMember(Name="persons",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Crm.Contact.CrmContactReference> Persons { get; set; }
    
        /// <summary>
        /// F&#246;retag som matchats
        /// </summary>
        [DataMember(Name="companies",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Crm.Contact.CrmContactReference> Companies { get; set; }
    
        /// <summary>
        /// D&#246;dsbon som matchats
        /// </summary>
        [DataMember(Name="deceasedEstates",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Crm.Contact.CrmContactReference> DeceasedEstates { get; set; }
    
    }
}