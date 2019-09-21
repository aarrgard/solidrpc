using System.Collections.Generic;
using SolidRpc.NpmGenerator.Types;
using System.Runtime.Serialization;
namespace SolidRpc.NpmGenerator.Types {
    /// <summary>
    /// successful operation
    /// </summary>
    public class NpmPackage {
        /// <summary>
        /// The files within the package
        /// </summary>
        [DataMember(Name="files")]
        public IEnumerable<NpmPackageFile> Files { get; set; }
    
    }
}