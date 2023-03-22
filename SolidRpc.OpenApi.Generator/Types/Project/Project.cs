using System.Collections.Generic;

namespace SolidRpc.OpenApi.Generator.Types.Project
{
    /// <summary>
    /// Represents a project
    /// </summary>
    public class Project
    {
        /// <summary>
        /// All the project files
        /// </summary>
        public IEnumerable<ProjectFile> Files { get; set; }

        /// <summary>
        /// The assets
        /// </summary>
        public ProjectAssets Assets { get; set; }
    }
}
