using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace SolidRpc.OpenApi.Generator.Types.Project
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectAssetsProject
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "restore")]
        public ProjectAssetsProjectRestore Restore { get; set; }
    }
}