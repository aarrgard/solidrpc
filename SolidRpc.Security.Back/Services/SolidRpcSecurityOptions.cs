using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Security.Back.Services
{
    /// <summary>
    /// Contains options for the 
    /// </summary>
    public class SolidRpcSecurityOptions
    {
        /// <summary>
        /// Should the static content be added when starting up
        /// </summary>
        public bool AddStaticContent { get; internal set; }

        /// <summary>
        /// The oidc client id.
        /// </summary>
        public string OidcClientId { get; set; }

        /// <summary>
        /// The oidc client security.
        /// </summary>
        public string OidcClientSecurity { get; set; }
    }
}
