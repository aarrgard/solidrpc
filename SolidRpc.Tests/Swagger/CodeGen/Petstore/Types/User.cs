using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class User {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="id",EmitDefaultValue=false)]
        public long Id { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="username",EmitDefaultValue=false)]
        public string Username { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="firstName",EmitDefaultValue=false)]
        public string FirstName { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="lastName",EmitDefaultValue=false)]
        public string LastName { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="email",EmitDefaultValue=false)]
        public string Email { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="password",EmitDefaultValue=false)]
        public string Password { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="phone",EmitDefaultValue=false)]
        public string Phone { get; set; }
    
        /// <summary>
        /// User Status
        /// </summary>
        [DataMember(Name="userStatus",EmitDefaultValue=false)]
        public int UserStatus { get; set; }
    
    }
}