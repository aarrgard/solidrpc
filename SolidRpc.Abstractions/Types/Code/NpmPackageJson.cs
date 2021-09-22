using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SolidRpc.Abstractions.Types.Code
{
    /// <summary>
    /// Represents a package.json file
    /// </summary>
    public class NpmPackageJson
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public NpmPackageJson()
        {
            Dependencies = new Dictionary<string, string>();
        }

        /// <summary>
        /// The name of the package
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The version of the package
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; set; }

        /// <summary>
        /// The description of the package
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// The main entry
        /// </summary>
        [DataMember(Name = "main")]
        public string Main { get; set; }

        /// <summary>
        /// The types entry
        /// </summary>
        [DataMember(Name = "types")]
        public string Types { get; set; }

        /// <summary>
        /// The license
        /// </summary>
        [DataMember(Name = "license")]
        public string License { get; set; }

        /// <summary>
        /// The dependencies
        /// </summary>
        [DataMember(Name = "dependencies")]
        public IDictionary<string, string> Dependencies { get; set; }

        /// <summary>
        /// The scripts
        /// </summary>
        [DataMember(Name = "scripts")]
        public Dictionary<string, string> Scripts { get; set; }
    }
}
