using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Security.Front
{
    /// <summary>
    /// The security options
    /// </summary>
    public class SolidRpcSecurityOptions : ISolidRpcSecurityOptions
    {

        /// <summary>
        /// The authority
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// The client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The client secret
        /// </summary>
        public string ClientSecret { get; set; }
    }
}
