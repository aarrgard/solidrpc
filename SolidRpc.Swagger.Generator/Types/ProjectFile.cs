using System.IO;

namespace SolidRpc.Swagger.Generator.Types
{
    /// <summary>
    /// Represents a project file
    /// </summary>
    public class ProjectFile
    {
        /// <summary>
        /// The filepath from the base of the project
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// The file content
        /// </summary>
        public Stream FileContent { get; set; } 
    }
}