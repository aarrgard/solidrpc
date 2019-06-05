using System.Collections.Generic;

namespace SolidRpc.OpenApi.Generator.Types
{
    /// <summary>
    /// Represents a project
    /// </summary>
    public class Project
    {
        /// <summary>
        /// All the project files
        /// </summary>
        public IEnumerable<ProjectFile> ProjectFiles { get; set; }
    }
}
