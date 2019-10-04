using System.CodeDom.Compiler;
using System.Runtime.Serialization;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// success
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class Settings {
        /// <summary>
        /// The authority to use
        /// </summary>
        [DataMember(Name="authority",EmitDefaultValue=false)]
        public string Authority { get; set; }
    
        /// <summary>
        /// The id of the client
        /// </summary>
        [DataMember(Name="client_id",EmitDefaultValue=false)]
        public string ClientId { get; set; }
    
        /// <summary>
        /// The response type
        /// </summary>
        [DataMember(Name="response_type",EmitDefaultValue=false)]
        public string ResponseType { get; set; }
    
        /// <summary>
        /// The scopes that should be supplied
        /// </summary>
        [DataMember(Name="scope",EmitDefaultValue=false)]
        public string Scope { get; set; }
    
        /// <summary>
        /// The the address to redirect to
        /// </summary>
        [DataMember(Name="redirect_uri",EmitDefaultValue=false)]
        public string RedirectUri { get; set; }
    
        /// <summary>
        /// The address to call when logging out.
        /// </summary>
        [DataMember(Name="post_logout_redirect_uri",EmitDefaultValue=false)]
        public string PostLogoutRedirectUri { get; set; }
    
    }
}