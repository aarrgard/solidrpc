using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Generator.Impl.Code.Binder
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


        public string ExternalUri { get; set; }

        public string ExternalDescription { get; set; }
    }
}
