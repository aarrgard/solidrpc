using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
using SolidRpc.Security.Types;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// 
    /// </summary>
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
        public IEnumerable<LoginProviderMeta> Meta { get; set; }
    
        /// <summary>
        /// The html for the login button
        /// </summary>
        [DataMember(Name="buttonHtml",EmitDefaultValue=false)]
        public string ButtonHtml { get; set; }
    
    }
}