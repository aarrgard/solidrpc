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
        /// Constructs a new instance
        /// </summary>
        public SettingsCodeGen()
        {
            UseAsyncAwaitPattern = true;
        }
        /// <summary>
        /// The swagger json.
        /// </summary>
        public string SwaggerSpec { get; set; }

        /// <summary>
        /// Specifies if we should use the async/await pattern. Setting this 
        /// to true will wrap all the return types with a generic Task and 
        /// add a cancellation token argument to all methods.
        /// </summary>
        public bool UseAsyncAwaitPattern { get; set; }
    }
}
