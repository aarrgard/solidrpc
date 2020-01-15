using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// success
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public class LoginProvider {
        /// <summary>
        /// The name of the provider
        /// </summary>
        [DataMember(Name="name",EmitDefaultValue=false)]
        public string Name { get; set; }
    
        /// <summary>
        /// The user status @ the provider. LoggedIn or NotLoggedIn
        /// </summary>
        [DataMember(Name="status",EmitDefaultValue=false)]
        public string Status { get; set; }
    
        /// <summary>
        /// The script uris
        /// </summary>
        [DataMember(Name="script",EmitDefaultValue=false)]
        public IEnumerable<Uri> Script { get; set; }
    
        /// <summary>
        /// The script uris
        /// </summary>
        [DataMember(Name="meta",EmitDefaultValue=false)]
        public IEnumerable<SolidRpc.Security.Types.LoginProviderMeta> Meta { get; set; }
    
        /// <summary>
        /// The html for the login button
        /// </summary>
        [DataMember(Name="buttonHtml",EmitDefaultValue=false)]
        public string ButtonHtml { get; set; }
    
    }
}