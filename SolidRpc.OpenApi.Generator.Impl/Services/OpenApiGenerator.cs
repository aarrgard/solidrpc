using ICSharpCode.SharpZipLib.Zip;
using SolidRpc.OpenApi.Generator.Impl.Csproj;
using SolidRpc.OpenApi.Generator.Services;
using SolidRpc.OpenApi.Generator.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Generator.Impl.Services
{
    public class OpenApiGenerator : IOpenApiGenerator
    {
        public Task<Project> CreateCodeFromOpenApiSpec(SettingsCodeGen settings, FileData swaggerFile, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<FileData> CreateOpenApiSpecFromCode(SettingsSpecGen settings, Project project, CancellationToken cancellationToken)
        {
            return Task.FromResult(OpenApiSpecGenerator.GenerateOpenApiSpec(settings, project));
        }

        public Task<SettingsCodeGen> GetSettingsCodeGenFromCsproj(FileData csproj, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<SettingsSpecGen> GetSettingsSpecGenFromCsproj(FileData csproj, CancellationToken cancellationToken = default(CancellationToken))
        {
            var csprojInfo = CsprojInfo.GetCsprojInfo(csproj.Filename, csproj.FileStream);
            return Task.FromResult(new SettingsSpecGen()
            {
                Title = csprojInfo.AssemblyName,
                Version = csprojInfo.OpenApiVersion ?? csprojInfo.Version ?? "1.0.0",
                Description = csprojInfo.OpenApiDescription ?? csprojInfo.Description,
                LicenseName = csprojInfo.OpenApiLicenseName ?? csprojInfo.PackageLicenseUrl,
                LicenseUrl = csprojInfo.OpenApiLicenseUrl ?? csprojInfo.PackageLicenseUrl,
                ContactEmail = csprojInfo.OpenApiContactEmail,
                ContactName = csprojInfo.OpenApiContactName ?? csprojInfo.Authors,
                ContactUrl = csprojInfo.OpenApiContactUrl ?? csprojInfo.PackageProjectUrl,
                ProjectNamespace = csprojInfo.ProjectNamespace,
                BasePath = csprojInfo.OpenApiBasePath
            });
        }

        public async Task<Project> ParseProject(FileData projectZip, CancellationToken cancellationToken)
        {
            if(!string.Equals(projectZip.ContentType,"application/zip",StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("Can only parse zip files - please use application/zip as content type.");
            }
            using (var zipStream = new ZipInputStream(projectZip.FileStream))
            {
                var project = new Project();
                await CreateProject(zipStream, project);
                return project;
            }
        }

        private async Task CreateProject(ZipInputStream zipStream, Project project)
        {
            var projectFiles = new List<ProjectFile>();
            ZipEntry ze;
            while((ze = zipStream.GetNextEntry()) != null)
            {
                var dir = "";
                var name = ze.Name;
                var slashIdx = ze.Name.LastIndexOf('/');
                if(slashIdx > -1)
                {
                    dir = ze.Name.Substring(0, slashIdx);
                    name = ze.Name.Substring(slashIdx + 1);
                }
                var ms = new MemoryStream();
                await zipStream.CopyToAsync(ms);
                projectFiles.Add(new ProjectFile()
                {
                    Directory = dir,
                    FileData = new FileData()
                    {
                        ContentType = "application/octet-stream",
                        Filename = name,
                        FileStream = new MemoryStream(ms.ToArray())
                    }
                });
            }
            project.ProjectFiles = projectFiles;
        }
    }
}
