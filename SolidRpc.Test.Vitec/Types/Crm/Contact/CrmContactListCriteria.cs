using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Test.Vitec.Types.Crm.Contact {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CrmContactListCriteria {
        /// <summary>
        /// Urval p&#229; anv&#228;ndareid
        /// </summary>
        [DataMember(Name="agentId",EmitDefaultValue=false)]
        public string AgentId { get; set; }
    
        /// <summary>
        /// Skapad fr&#229;n och med
        /// </summary>
        [DataMember(Name="createdAtFrom",EmitDefaultValue=false)]
        public DateTimeOffset CreatedAtFrom { get; set; }
    
        /// <summary>
        /// Skapad till och med
        /// </summary>
        [DataMember(Name="createdAtTo",EmitDefaultValue=false)]
        public DateTimeOffset CreatedAtTo { get; set; }
    
        /// <summary>
        /// &#196;ndrad fr&#229;n och med
        /// </summary>
        [DataMember(Name="changedAtFrom",EmitDefaultValue=false)]
        public DateTimeOffset ChangedAtFrom { get; set; }
    
        /// <summary>
        /// &#196;ndrad till och med
        /// </summary>
        [DataMember(Name="changedAtTo",EmitDefaultValue=false)]
        public DateTimeOffset ChangedAtTo { get; set; }
    
        /// <summary>
        /// Egendefinerat f&#228;ltnamn
        /// </summary>
        [DataMember(Name="customFieldName",EmitDefaultValue=false)]
        public string CustomFieldName { get; set; }
    
        /// <summary>
        /// Egendefinerat f&#228;ltv&#228;rde
        /// </summary>
        [DataMember(Name="customFieldValue",EmitDefaultValue=false)]
        public string CustomFieldValue { get; set; }
    
    }
}