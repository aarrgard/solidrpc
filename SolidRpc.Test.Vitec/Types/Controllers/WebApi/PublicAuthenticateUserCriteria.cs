using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Test.Vitec.Types.Controllers.WebApi {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class PublicAuthenticateUserCriteria {
        /// <summary>
        /// Kundid
        /// </summary>
        [DataMember(Name="customerId",EmitDefaultValue=false)]
        public string CustomerId { get; set; }
    
        /// <summary>
        /// Anv�ndarnamn
        /// </summary>
        [DataMember(Name="userName",EmitDefaultValue=false)]
        public string UserName { get; set; }
    
        /// <summary>
        /// L�senord
        /// </summary>
        [DataMember(Name="password",EmitDefaultValue=false)]
        public string Password { get; set; }
    
    }
}