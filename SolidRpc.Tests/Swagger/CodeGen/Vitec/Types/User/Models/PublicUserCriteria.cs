using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.User.Models {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PublicUserCriteria {
        /// <summary>
        /// Anv&#228;ndarid
        /// </summary>
        [DataMember(Name="userId",EmitDefaultValue=false)]
        public string UserId { get; set; }
    
        /// <summary>
        /// Text som filtrerar p&#229; namn eller titel
        /// </summary>
        [DataMember(Name="searchText",EmitDefaultValue=false)]
        public string SearchText { get; set; }
    
        /// <summary>
        /// Kundid
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
    }
}