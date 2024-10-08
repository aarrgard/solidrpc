using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.AzFunctions.Functions
{
    public class AzFunctionEmitSettings
    {
        /// <summary>
        /// The usings directive
        /// </summary>
        public string Usings { get; set; }

        /// <summary>
        /// The name attribute
        /// </summary>
        public string NameAttribute { get; set; }

        /// <summary>
        /// The http request class
        /// </summary>
        public string HttpRequestClass { get; set; }

        /// <summary>
        /// The http response class
        /// </summary>
        public string HttpResponseClass { get; set; }
    }
}
