using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SolidRpc.Abstractions.Types.Code
{
    /// <summary>
    /// successful operation
    /// </summary>
    public class NpmPackage {
        /// <summary>
        /// The package name(folder name)
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The files within the package
        /// </summary>
        [DataMember(Name="files")]
        public IEnumerable<NpmPackageFile> Files { get; set; }
    }
}