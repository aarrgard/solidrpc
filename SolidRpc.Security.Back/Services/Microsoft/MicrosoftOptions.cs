using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Security.Back.Services.Microsoft
{
    /// <summary>
    /// Contains options for the Microsoft integration
    /// </summary>
    public class MicrosoftOptions
    {
        /// <summary>
        /// The tenant that we use at microsoft to authenticate
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// The  ClientId / application id.
        /// </summary>
        public Guid ClientID { get; set; }

        /// <summary>
        /// The  application id(ClientId).
        /// </summary>
        public string ClientSecret { get; set; }
    }
}
