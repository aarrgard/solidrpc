using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CRM.Usergroup {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class UserGroup {
        /// <summary>
        /// Anv�ndar
        /// </summary>
        [DataMember(Name="userGroupId",EmitDefaultValue=false)]
        public string UserGroupId { get; set; }
    
        /// <summary>
        /// Kundid
        /// </summary>
        [DataMember(Name="customerIds",EmitDefaultValue=false)]
        public IEnumerable<string> CustomerIds { get; set; }
    
        /// <summary>
        /// Namn
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// Anv�ndare
        /// </summary>
        [DataMember(Name="users",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CRM.Usergroup.User> Users { get; set; }
    
    }
}