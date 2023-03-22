using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Generator.Types.Project
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectAssetsTargetRef
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "dependencies")]
        public ProjectAssetsTargetRefDependencies Dependencies { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "compile")]
        public ProjectAssetsTargetRefRuntimes Compile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "runtime")]
        public ProjectAssetsTargetRefRuntimes Runtime { get; set; }
    }
}