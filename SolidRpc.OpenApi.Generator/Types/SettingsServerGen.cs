using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Generator.Types
{
    /// <summary>
    /// Settings for generating server bindings
    /// </summary>
    public class SettingsServerGen : SettingsGen
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public SettingsServerGen()
        {
        }

        /// <summary>
        /// The project base fix to apply to the working directory.
        /// </summary>
        public string ProjectBaseFix { get; set; }
    }
}
