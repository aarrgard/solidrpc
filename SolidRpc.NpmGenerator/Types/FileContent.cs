using System.IO;
using System.Runtime.Serialization;
namespace SolidRpc.NpmGenerator.Types {
    /// <summary>
    /// successful operation
    /// </summary>
    public class FileContent {
        /// <summary>
        /// The binary file content
        /// </summary>
        [DataMember(Name="content")]
        public Stream Content { get; set; }
    
        /// <summary>
        /// The content type
        /// </summary>
        [DataMember(Name="contentType")]
        public string ContentType { get; set; }
    
        /// <summary>
        /// The file name
        /// </summary>
        [DataMember(Name="fileName")]
        public string FileName { get; set; }
    
    }
}