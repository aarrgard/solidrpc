using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Generator.Types.Project
{
    /// <summary>
    /// The parsed assets file
    /// </summary>
    public class ProjectAssets
    {
        /// <summary>
        /// The version
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; set; }

        /// <summary>
        /// The targets
        /// </summary>
        [DataMember(Name = "targets")]
        public ProjectAssetsTargets Targets { get; set; }

        /// <summary>
        /// The libraries
        /// </summary>
        [DataMember(Name = "libraries")]
        public ProjectAssetsLibraries Libraries { get; set; }

        /// <summary>
        /// The priject
        /// </summary>
        [DataMember(Name = "project")]
        public ProjectAssetsProject Project { get; set; }
    }
}