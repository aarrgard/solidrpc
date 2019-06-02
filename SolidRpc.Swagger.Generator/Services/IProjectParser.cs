using SolidRpc.Swagger.Generator.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
        Task<Project> ParseProject(Stream projectZip, CancellationToken cancellationToken);
    }
}
