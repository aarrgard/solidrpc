using System;

namespace SolidRpc.Tests.Swagger.SpecGen.HttpRequestArgs.Types
{
    /// <summary>
    /// ComplexType1
    /// </summary>
    public class HttpRequest
    {
        /// <summary>
        /// The method used
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Uri
        /// </summary>
        public Uri Uri { get; set; }
    }
}
