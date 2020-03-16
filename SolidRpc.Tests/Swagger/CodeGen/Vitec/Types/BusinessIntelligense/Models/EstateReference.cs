using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Models.Api;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class EstateReference {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public string Id { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="status",EmitDefaultValue=false)]
        public DescriptiveId1_Int16 Status { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models.EconomicAddress Address { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="isNewHome",EmitDefaultValue=false)]
        public bool? IsNewHome { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="responsibleAgentName",EmitDefaultValue=false)]
        public string ResponsibleAgentName { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="contractDay",EmitDefaultValue=false)]
        public System.DateTimeOffset? ContractDay { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="advertisedAt",EmitDefaultValue=false)]
        public System.DateTimeOffset? AdvertisedAt { get; set; }
    
    }
}