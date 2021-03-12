using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.SwaggerUI
{
    /// <summary>
    /// Defines some swagger options
    /// </summary>
    public class SwaggerOptions
    {
        /// <summary>
        /// The default open api spec
        /// </summary>
        public string DefaultOpenApiSpec { get; set; }

        /// <summary>
        /// The oauth client id.
        /// </summary>
        public string OAuthClientId { get; set; }

        /// <summary>
        /// The oauth client secret
        /// </summary>
        public string OAuthClientSecret { get; set; }
    }
}
