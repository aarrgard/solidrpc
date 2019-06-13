using SolidRpc.OpenApi.Generator.Types;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Generator.Services
{
    /// <summary>
    /// The project parser is responsible for parsing project files into representations
    /// that can be analyzed by the swagger code generators.
    /// </summary>
    public interface IOpenApiGenerator
    {
        /// <summary>
        /// Parses the supplied project zip into a project representation.
        /// </summary>
        /// <param name="projectZip"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Project> ParseProject(
            FileData projectZip, 
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates a swagger specification from supplied project.
        /// </summary>
        /// <param name="settings">The settings for generating the spec</param>
        /// <param name="project">The project to analyze</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileData> CreateOpenApiSpecFromCode(
            SettingsSpecGen settings, 
            Project project, 
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates a swagger specification from supplied project.
        /// </summary>
        /// <param name="settings">The settings for generating the code</param>
        /// <param name="swaggerFile">The swagger file to analyze</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Project> CreateCodeFromOpenApiSpec(
            SettingsCodeGen settings, 
            FileData swaggerFile, 
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the settings for the code generation based
        /// on the settings in supplied csproj file.
        /// </summary>
        /// <param name="csproj">The csproj file</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SettingsCodeGen> GetSettingsCodeGenFromCsproj(
            FileData csproj,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the settings for the spec generation based
        /// on the settings in supplied csproj file.
        /// </summary>
        /// <param name="csproj">The csproj file</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SettingsSpecGen> GetSettingsSpecGenFromCsproj(
            FileData csproj,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
