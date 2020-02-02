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
        /// Constructs a new instance
        /// </summary>
        public SolidRpcSecurityOptions()
        {
            AddStaticContent = true;
        }
        /// <summary>
        /// Should the static content be added when starting up
        /// </summary>
        public bool AddStaticContent { get; set; }

        /// <summary>
        /// The oidc client id.
        /// </summary>
        public string OidcClientId { get; set; }

        /// <summary>
        /// The oidc client secret.
        /// </summary>
        public string OidcClientSecret { get; set; }
    }
}
