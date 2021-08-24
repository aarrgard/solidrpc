using System.Runtime.Serialization;
namespace SolidRpc.Abstractions.Types.Code {
    /// <summary>
    /// 
    /// </summary>
    public class NpmPackageFile {
        /// <summary>
        /// The file path within the package
        /// </summary>
        [DataMember(Name="filePath")]
        public string FilePath { get; set; }
    
        /// <summary>
        /// The file content(binary content not supported)
        /// </summary>
        [DataMember(Name="content")]
        public string Content { get; set; }
    
    }
}