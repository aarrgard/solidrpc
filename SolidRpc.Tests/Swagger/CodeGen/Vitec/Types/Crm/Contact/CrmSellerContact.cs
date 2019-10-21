using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Crm.Contact {
    /// <summary>
    /// OK
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CrmSellerContact {
        /// <summary>
        /// Kontaktid
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Bost&#228;derna som kontakten &#228;r s&#228;ljare p&#229;.
        /// </summary>
        [DataMember(Name="sellerOn",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Crm.Contact.CrmSellerRelation> SellerOn { get; set; }
    
    }
}