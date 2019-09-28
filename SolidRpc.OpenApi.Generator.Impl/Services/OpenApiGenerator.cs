using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.OpenApi.Generator.Impl.Csproj;
using SolidRpc.OpenApi.Generator.Services;
using SolidRpc.OpenApi.Generator.Types;
using SolidRpc.OpenApi.Model;
using SolidRpc.OpenApi.Model.CSharp;
using SolidRpc.OpenApi.Model.Generator.V2;
using SolidRpc.OpenApi.Model.Generator.V3;
using SolidRpc.OpenApi.Model.V2;
using SolidRpc.OpenApi.Model.V3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Generator.Impl.Services
{
    public class OpenApiGenerator : IOpenApiGenerator
    {
        public OpenApiGenerator(IOpenApiParser openApiParser)
        {
            OpenApiParser = openApiParser;
        }
        private IOpenApiParser OpenApiParser { get; }

        public Task<Project> CreateCodeFromOpenApiSpec(SettingsCodeGen codeSettings, CancellationToken cancellationToken)
        {
            var model = OpenApiParser.ParseSpec(null, null, codeSettings.SwaggerSpec);
            ICSharpRepository codeRepo;
            if (model is SwaggerObject v2)
            {
                codeRepo = new OpenApiCodeGeneratorV2(v2, CopySettings<Model.Generator.SettingsCodeGen>(codeSettings)).GenerateCode();
            }
            else if (model is OpenAPIObject v3)
            {
                codeRepo = new OpenApiCodeGeneratorV3(v3, CopySettings<Model.Generator.SettingsCodeGen>(codeSettings)).GenerateCode();
            }
            else
            {
                throw new Exception("Cannot parse swagger json.");
            }

            var codeWriter = new CodeWriterZip(codeSettings.ProjectNamespace);
            codeRepo.WriteCode(codeWriter);
            codeWriter.Close();
            codeWriter.ZipOutputStream.Close();

            var projectZip = new FileData()
            {
                ContentType = "application/zip",
                Filename = "project.zip",
                FileStream = new MemoryStream(codeWriter.MemoryStream.ToArray())
            };


            return ParseProjectZip(projectZip, cancellationToken);
        }

        private T CopySettings<T>(object codeSettings) where T : new()
        {
            var s = new T();
            foreach(var pTarget in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var pSource = codeSettings.GetType().GetProperty(pTarget.Name);
                pTarget.SetValue(s, pSource.GetValue(codeSettings));
            }
            return s;
        }

        public Task<FileData> CreateOpenApiSpecFromCode(SettingsSpecGen settings, Project project, CancellationToken cancellationToken)
        {
            var cSharpRepository = CSharpParser.ParseProject(project);
            IOpenApiSpec openApiSpec;
            switch (settings.OpenApiVersion)
            {
                case "2.0":
                    openApiSpec = new OpenApiSpecGeneratorV2(CopySettings<Model.Generator.SettingsSpecGen>(settings)).CreateSwaggerSpec(OpenApiSpecResolverDummy.Instance, cSharpRepository);
                    break;
                default:
                    throw new Exception("Cannot handle swagger version:" + settings.OpenApiVersion);
            }

            return Task.FromResult(new FileData()
            {
                ContentType = "application/json",
                FileStream = new MemoryStream(Encoding.UTF8.GetBytes(openApiSpec.WriteAsJsonString(true))),
                Filename = $"{settings.ProjectNamespace}.json"
            });
        }

        public Task<SettingsCodeGen> GetSettingsCodeGenFromCsproj(FileData csproj, CancellationToken cancellationToken = default(CancellationToken))
        {
            var csprojInfo = CsprojInfo.GetCsprojInfo(csproj.Filename, csproj.FileStream);
            return Task.FromResult(new SettingsCodeGen()
            {
                ProjectNamespace = csprojInfo.RootNamespace
            });
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
                ProjectNamespace = csprojInfo.RootNamespace,
                BasePath = csprojInfo.OpenApiBasePath
            });
        }

        public async Task<Project> ParseProjectZip(FileData projectZip, CancellationToken cancellationToken)
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

        public async Task<FileData> CreateProjectZip(Project project, CancellationToken cancellationToken = default(CancellationToken))
        {
            var ms = new MemoryStream();
            using (var zos = new ZipOutputStream(ms))
            {
                foreach (var f in project.ProjectFiles)
                {
                    var ze = new ZipEntry($"{f.Directory}/{f.FileData.Filename}");
                    zos.PutNextEntry(ze);
                    await f.FileData.FileStream.CopyToAsync(zos);
                    zos.CloseEntry();
                }
            }
            return new FileData()
            {
                ContentType = "application/octet-stream",
                Filename = "project.zip",
                FileStream = new MemoryStream(ms.ToArray())
            };
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
