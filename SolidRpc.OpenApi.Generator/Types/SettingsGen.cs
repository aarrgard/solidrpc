using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Generator.Types
{
    /// <summary>
    /// Base class for the settings
    /// </summary>
    public abstract class SettingsGen
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public SettingsGen()
        {
            TypeNamespace = "Types";
            ServiceNamespace = "Services";
            SecurityNamespace = "Security";
        }
        /// <summary>
        /// The namespace that the project belongs to. This namespace 
        /// will not be included in the type references.
        /// </summary>
        public string ProjectNamespace { get; set; }

        /// <summary>
        /// The namespace that will be added to the project namespace
        /// before adding the type or service namespace.
        /// </summary>
        public string CodeNamespace { get; set; }

        /// <summary>
        /// The namespace to append to the root namespace for all the types
        /// </summary>
        public string TypeNamespace { get; set; }

        /// <summary>
        /// The namespace to append to the root namespace for all the services(interfaces)
        /// </summary>
        public string ServiceNamespace { get; set; }

        /// <summary>
        /// The namespace where the security attributes are stored
        /// </summary>
        public string SecurityNamespace { get; set; }

    }
}
