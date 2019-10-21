using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Types.Crm.Contact {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CrmBuyerContact {
        /// <summary>
        /// Kontaktid
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Bost&#228;derna som kontakten &#228;r k&#246;pare p&#229;.
        /// </summary>
        [DataMember(Name="buyerOn",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Test.Vitec.Types.Crm.Contact.CrmBuyerRelation> BuyerOn { get; set; }
    
    }
}