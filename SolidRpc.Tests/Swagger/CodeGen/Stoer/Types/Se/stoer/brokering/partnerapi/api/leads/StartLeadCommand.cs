using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Stoer.Types.Se.stoer.brokering.partnerapi.api.leads {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class StartLeadCommand {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="givenName",EmitDefaultValue=false)]
        public string GivenName { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="familyName",EmitDefaultValue=false)]
        public string FamilyName { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="email",EmitDefaultValue=false)]
        public string Email { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="phone",EmitDefaultValue=false)]
        public string Phone { get; set; }
    
        /// <summary>
        /// An optional origin for the lead. Allowed origin can be gotten on /config/origins (GET)
        /// </summary>
        [DataMember(Name="origin",EmitDefaultValue=false)]
        public string Origin { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="organization",EmitDefaultValue=false)]
        public string Organization { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="organizationUnit",EmitDefaultValue=false)]
        public string OrganizationUnit { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="individual",EmitDefaultValue=false)]
        public string Individual { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="leadType",EmitDefaultValue=false)]
        public string LeadType { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="other",EmitDefaultValue=false)]
        public string Other { get; set; }
    
        /// <summary>
        /// An optional comment about the lead.
        /// </summary>
        [DataMember(Name="comment",EmitDefaultValue=false)]
        public string Comment { get; set; }
    
        /// <summary>
        /// The external ID of the lead. If included, this will be used to filter out duplicate requests.
        /// </summary>
        [DataMember(Name="externalId",EmitDefaultValue=false)]
        public string ExternalId { get; set; }
    
    }
}