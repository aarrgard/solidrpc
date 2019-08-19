using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
using SolidRpc.Security.Types;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// success
    /// </summary>
    public class LoginProvider {
        /// <summary>
        /// The name of the provider
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }
    
        /// <summary>
        /// The user status @ the provider. LoggedIn or NotLoggedIn
        /// </summary>
        [DataMember(Name="status")]
        public string Status { get; set; }
    
        /// <summary>
        /// The script uris
        /// </summary>
        [DataMember(Name="script")]
        public IEnumerable<Uri> Script { get; set; }
    
        /// <summary>
        /// The script uris
        /// </summary>
        [DataMember(Name="meta")]
        public IEnumerable<LoginProviderMeta> Meta { get; set; }
    
        /// <summary>
        /// The html for the login button
        /// </summary>
        [DataMember(Name="buttonHtml")]
        public string ButtonHtml { get; set; }
    
    }
}