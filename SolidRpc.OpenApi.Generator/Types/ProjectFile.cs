using System.IO;

namespace SolidRpc.OpenApi.Generator.Types
{
    /// <summary>
    /// Represents a project file
    /// </summary>
    public class ProjectFile
    {
        /// <summary>
        /// The directory where the file resides
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// The file data
        /// </summary>
        public FileData FileData { get; set; } 
    }
}