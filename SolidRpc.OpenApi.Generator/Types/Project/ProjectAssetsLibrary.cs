using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace SolidRpc.OpenApi.Generator.Types.Project
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectAssetsLibrary
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "sha512")]
        public string Sha512 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "path")]
        public string Path { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "files")]
        public IEnumerable<string> Files { get; set; }
    }
}