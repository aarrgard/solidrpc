using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Generator.Types
{
    /// <summary>
    /// Settings for generating code from a swagger file.
    /// </summary>
    public class SettingsCodeGen : SettingsGen
    {
        /// <summary>
        /// The swagger json.
        /// </summary>
        public string SwaggerSpec { get; set; }

        /// <summary>
        /// The output path. May be a folder or zip.
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Specifies if we should use the async/await pattern. Setting this 
        /// to true will wrap all the return types with a generic Task and 
        /// add a cancellation token argument to all methods.
        /// </summary>
        public bool UseAsyncAwaitPattern { get; set; }
    }
}
