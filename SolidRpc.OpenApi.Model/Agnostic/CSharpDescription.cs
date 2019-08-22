using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.Agnostic
{
    /// <summary>
    /// Represents a charp description
    /// </summary>
    public class CSharpDescription
    {
        /// <summary>
        /// The summary
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// The external uri
        /// </summary>
        public string ExternalUri { get; set; }

        /// <summary>
        /// The external desciption
        /// </summary>
        public string ExternalDescription { get; set; }
    }
}
