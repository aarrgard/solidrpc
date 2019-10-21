using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Cms;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Cms.Estate {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class CmsProjectBaseInformation {
        /// <summary>
        /// Status p&#229; projektet
        /// </summary>
        [DataMember(Name="status",EmitDefaultValue=false)]
        public string Status { get; set; }
    
        /// <summary>
        /// Namn p&#229; projektet
        /// </summary>
        [DataMember(Name="projectName",EmitDefaultValue=false)]
        public string ProjectName { get; set; }
    
        /// <summary>
        /// Datum f&#246;r s&#228;ljstart
        /// </summary>
        [DataMember(Name="saleStart",EmitDefaultValue=false)]
        public DateTimeOffset SaleStart { get; set; }
    
        /// <summary>
        /// Prelimin&#228;r tidpunkt f&#246;r inflytt
        /// </summary>
        [DataMember(Name="preliminaryAccess",EmitDefaultValue=false)]
        public string PreliminaryAccess { get; set; }
    
        /// <summary>
        /// Namn p&#229; producent
        /// </summary>
        [DataMember(Name="producer",EmitDefaultValue=false)]
        public string Producer { get; set; }
    
        /// <summary>
        /// Projektets adress
        /// </summary>
        [DataMember(Name="address",EmitDefaultValue=false)]
        public CmsEstateAddress Address { get; set; }
    
    }
}