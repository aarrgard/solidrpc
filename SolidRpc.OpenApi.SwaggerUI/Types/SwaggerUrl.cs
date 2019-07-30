using System;

namespace SolidRpc.OpenApi.SwaggerUI.Types
{
    /// <summary>
    /// Represent a swagger url
    /// </summary>
    public class SwaggerUrl
    {
        /// <summary>
        /// The name of the url
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The url
        /// </summary>
        public Uri Url { get; set; }
    }
}