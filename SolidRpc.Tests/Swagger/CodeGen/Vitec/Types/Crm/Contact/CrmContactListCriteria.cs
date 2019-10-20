using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Crm.Contact {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CrmContactListCriteria {
        /// <summary>
        /// Urval p� anv�ndareid
        /// </summary>
        [DataMember(Name="agentId",EmitDefaultValue=false)]
        public string AgentId { get; set; }
    
        /// <summary>
        /// Skapad fr�n och med
        /// </summary>
        [DataMember(Name="createdAtFrom",EmitDefaultValue=false)]
        public DateTimeOffset CreatedAtFrom { get; set; }
    
        /// <summary>
        /// Skapad till och med
        /// </summary>
        [DataMember(Name="createdAtTo",EmitDefaultValue=false)]
        public DateTimeOffset CreatedAtTo { get; set; }
    
        /// <summary>
        /// �ndrad fr�n och med
        /// </summary>
        [DataMember(Name="changedAtFrom",EmitDefaultValue=false)]
        public DateTimeOffset ChangedAtFrom { get; set; }
    
        /// <summary>
        /// �ndrad till och med
        /// </summary>
        [DataMember(Name="changedAtTo",EmitDefaultValue=false)]
        public DateTimeOffset ChangedAtTo { get; set; }
    
        /// <summary>
        /// Egendefinerat f�ltnamn
        /// </summary>
        [DataMember(Name="customFieldName",EmitDefaultValue=false)]
        public string CustomFieldName { get; set; }
    
        /// <summary>
        /// Egendefinerat f�ltv�rde
        /// </summary>
        [DataMember(Name="customFieldValue",EmitDefaultValue=false)]
        public string CustomFieldValue { get; set; }
    
    }
}