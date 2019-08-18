using System.Runtime.Serialization;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Types {
    /// <summary>
    /// 
    /// </summary>
    public class User {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="id")]
        public long Id { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="username")]
        public string Username { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="firstName")]
        public string FirstName { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="lastName")]
        public string LastName { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="email")]
        public string Email { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="password")]
        public string Password { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="phone")]
        public string Phone { get; set; }
    
        /// <summary>
        /// User Status
        /// </summary>
        [DataMember(Name="userStatus")]
        public int UserStatus { get; set; }
    
    }
}