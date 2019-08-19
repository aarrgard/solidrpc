using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Security.Impl.Services.Google
{
    /// <summary>
    /// Contains options for the google integration
    /// </summary>
    public class GoogleOptions
    {
        /// <summary>
        /// The client id
        /// </summary>
        public string ClientID { get; set; }

        /// <summary>
        /// The signin scopes.
        /// </summary>
        public string SigninScopes { get; set; } = "profile email";
    }
}
