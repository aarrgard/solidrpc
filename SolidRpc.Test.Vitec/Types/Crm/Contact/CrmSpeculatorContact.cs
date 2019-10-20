using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.Crm.Contact {
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
        /// Bost�derna som kontakten �r och har varit spekulant p�.
        /// </summary>
        [DataMember(Name="speculatorOn",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Crm.Contact.CrmSpeculatorRelation> SpeculatorOn { get; set; }
    
    }
}