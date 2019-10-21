using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Crm.Contact {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CrmSpeculatorContact {
        /// <summary>
        /// Kontaktid
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Bost&#228;derna som kontakten &#228;r och har varit spekulant p&#229;.
        /// </summary>
        [DataMember(Name="speculatorOn",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Crm.Contact.CrmSpeculatorRelation> SpeculatorOn { get; set; }
    
    }
}