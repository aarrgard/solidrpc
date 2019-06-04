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
    public interface ISwaggerGenerator
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
        /// <param name="settings">The settings for generating the spec</param>
        /// <param name="project">The project to analyze</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Stream> CreateSwaggerSpec(SettingsSpecGen settings, Project project, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a swagger specification from supplied project.
        /// </summary>
        /// <param name="settings">The settings for generating the code</param>
        /// <param name="swaggerFile">The swagger file to analyze</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Project> CreateSwaggerCode(SettingsCodeGen settings, FileData swaggerFile, CancellationToken cancellationToken);
    }
}
