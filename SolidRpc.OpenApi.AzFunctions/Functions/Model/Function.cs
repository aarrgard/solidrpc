using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Model
{
    /// <summary>
    /// Represents a function.
    /// </summary>
    public class Function
    {
        /// <summary>
        /// The generator
        /// </summary>
        [DataMember(Name = "generatedBy", EmitDefaultValue = false)]
        public string GeneratedBy { get; set; }

        /// <summary>
        /// The source
        /// </summary>
        [DataMember(Name = "configurationSource", EmitDefaultValue = false)]
        public string ConfigurationSource { get; set; }

        /// <summary>
        /// Is the function disabled
        /// </summary>
        [DataMember(Name = "disabled", EmitDefaultValue = false)]
        public bool Disabled { get; set; }

        /// <summary>
        /// The bindings
        /// </summary>
        [DataMember(Name = "bindings", EmitDefaultValue = false)]
        public IEnumerable<Binding> Bindings { get; set; }

        /// <summary>
        /// The script file
        /// </summary>
        [DataMember(Name = "scriptFile", EmitDefaultValue = false)]
        public string ScriptFile { get; set; }

        /// <summary>
        /// The script file
        /// </summary>
        [DataMember(Name = "entryPoint", EmitDefaultValue = false)]
        public string EntryPoint { get; set; }

    }
}
