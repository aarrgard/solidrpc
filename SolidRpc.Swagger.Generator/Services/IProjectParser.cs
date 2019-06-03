using SolidRpc.Swagger.Generator.Types;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Swagger.Generator.Services
{
    /// <summary>
    /// The project parser is responsible for parsing project files into representations
    /// that can be analyzed by the swagger code generators.
    /// </summary>
    public interface IProjectFileParser
    {
        /// <summary>
        /// Parses the supplied project zip into a project representation.
        /// </summary>
        /// <param name="projectZip"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Project> ParseProject(FileData projectZip, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a swagger specification from supplied project.
        /// </summary>
        /// <param name="project">The project to analyze</param>
        /// <param name="openApiVersion">The version of swagger specification to create</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Stream> CreateSwaggerSpec(Project project, string openApiVersion, CancellationToken cancellationToken);
    }
}
