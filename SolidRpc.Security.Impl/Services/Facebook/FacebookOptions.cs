using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Security.Impl.Services.Facebook
{
    /// <summary>
    /// The options for using the facebook login
    /// </summary>
    public class FacebookOptions
    {
        /// <summary>
        /// The application id for the registered app. Register apps @ https://developers.facebook.com
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// The app secret.
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// The requested login scopes
        /// </summary>
        public string RequestedScopes { get; set; } = "public_profile,email";
    }
}
