using SolidRpc.OpenApi.Generator.Services;
using SolidRpc.OpenApi.Generator.Types;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Generator.Impl.Services
{
    public class OpenApiGenerator : IOpenApiGenerator
    {
        public Task<Project> CreateSwaggerCode(SettingsCodeGen settings, FileData swaggerFile, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> CreateSwaggerSpec(SettingsSpecGen settings, Project project, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Project> ParseProject(FileData projectZip, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
