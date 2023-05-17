using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.OpenApi.Generator.Impl.Csproj;
using SolidRpc.OpenApi.Generator.Services;
using SolidRpc.OpenApi.Generator.Types;
using SolidRpc.OpenApi.Generator.Types.Project;
using SolidRpc.OpenApi.Model.CSharp;
using SolidRpc.OpenApi.Model.CSharp.Impl;
using SolidRpc.OpenApi.Model.Generator.V2;
using SolidRpc.OpenApi.Model.Generator.V3;
using SolidRpc.OpenApi.Model.V2;
using SolidRpc.OpenApi.Model.V3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Generator.Impl.Services
{
    public class OpenApiGenerator : IOpenApiGenerator
    {
        public OpenApiGenerator(IOpenApiParser openApiParser, ISerializerFactory serializerFactory)
        {
            OpenApiParser = openApiParser;
            SerializerFactory = serializerFactory;
        }
        private IOpenApiParser OpenApiParser { get; }
        private ISerializerFactory SerializerFactory { get; }

        public Task<Project> CreateCodeFromOpenApiSpec(SettingsCodeGen codeSettings, Project project, CancellationToken cancellationToken)
        {
            var openApiSpecResolver = new OpenApiSpecResolverProject(OpenApiParser, project);
            IOpenApiSpec model;
            if (!openApiSpecResolver.TryResolveApiSpec(codeSettings.SwaggerSpecFile, out model))
            {
                throw new Exception($"Failed to find/parse file {codeSettings.SwaggerSpecFile}");
            }
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
                    var openApiSpecResolver = new OpenApiSpecResolverProject(OpenApiParser, project);
                    openApiSpec = new OpenApiSpecGeneratorV2(CopySettings<Model.Generator.SettingsSpecGen>(settings)).CreateSwaggerSpec(openApiSpecResolver, cSharpRepository);
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

        public Task<SettingsCodeGen> GetSettingsCodeGenFromCsproj(FileData csproj, CancellationToken cancellationToken = default)
        {
            var csprojInfo = CsprojInfo.GetCsprojInfo(csproj.Filename, csproj.FileStream);
            return Task.FromResult(new SettingsCodeGen()
            {
                ProjectNamespace = csprojInfo.RootNamespace
            });
        }

        public Task<SettingsSpecGen> GetSettingsSpecGenFromCsproj(FileData csproj, CancellationToken cancellationToken = default)
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

        public async Task<FileData> CreateProjectZip(Project project, CancellationToken cancellationToken = default)
        {
            var ms = new MemoryStream();
            using (var zos = new ZipOutputStream(ms))
            {
                foreach (var f in project.Files)
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

                // parse assets file
                if(string.Equals("obj/project.assets.json", ze.Name, StringComparison.InvariantCultureIgnoreCase)) 
                {
                    ms.Position = 0;
                    SerializerFactory.DeserializeFromStream(ms, out ProjectAssets projectAssets);
                    project.Assets = projectAssets;
                }
            }
            project.Files = projectFiles;
        }

        public async Task<Project> CreateServerCode(SettingsServerGen settings, Project project, CancellationToken cancellationToken = default)
        {
            var serverCode = (ICSharpRepository)new CSharpRepository();
            foreach (var projectFile in project.Files ?? new ProjectFile[0])
            {
                var fileName = projectFile.FileData.Filename;
                if (!fileName.EndsWith(".dll"))
                {
                    continue;
                }
                var ns = fileName.Substring(0, fileName.Length - 4);

                CreateServerCode(serverCode, ns, projectFile.FileData.FileStream);
            }

            var codeWriter = new CodeWriterZip("");
            serverCode.WriteCode(codeWriter);
            codeWriter.Close();
            codeWriter.ZipOutputStream.Close();

            using (var zipStream = new ZipInputStream(new MemoryStream(codeWriter.MemoryStream.ToArray())))
            {
                project = new Project();
                await CreateProject(zipStream, project);
                AddProjectFile(project, ".SolidRpcExtensions.cs");
                return project;
            }
        }

        private void AddProjectFile(Project project, string resourceSuffix)
        {
            var resName = GetType().Assembly.GetManifestResourceNames().Single(o => o.EndsWith(resourceSuffix));
            project.Files = project.Files.Union(new[] { new ProjectFile() {
                Directory = "Microsoft/Extensions/DependencyInjection",
                FileData = new FileData()
                {
                    ContentType = "text/plain",
                    Filename = resourceSuffix.Substring(1),
                    FileStream = GetType().Assembly.GetManifestResourceStream(resName)
                }
            } }).ToList();
        }

        private void CreateServerCode(ICSharpRepository serverCode, string ns, Stream dllStream)
        {
            var nsNoDots = ns.Replace(".", "");
            var c = serverCode.GetClass($"Microsoft.Extensions.DependencyInjection.{nsNoDots}Extensions");
            c.SetModifier("public");
            c.SetModifier("static");

            var sns = new CSharpNamespace(serverCode, "System");
            var csns = new CSharpNamespace(serverCode, "Microsoft.Extensions.DependencyInjection.SolidRpcExtensions");
            var ipc = new CSharpInterface(csns, "IProxyConfig", null);

            var configureArg = serverCode.GetClass($"System.Func<{ipc.FullName},{ipc.FullName}>");
            var sc = serverCode.GetType($"Microsoft.Extensions.DependencyInjection.IServiceCollection");
            var addServiceCollection = new CSharpMethod(c, $"Add{nsNoDots}", sc);
            addServiceCollection.AddMember(new CSharpModifier(addServiceCollection, "public"));
            addServiceCollection.AddMember(new CSharpModifier(addServiceCollection, "static"));
            addServiceCollection.AddMember(new CSharpMethodParameter(addServiceCollection, "sc", sc, true, false));
            addServiceCollection.AddMember(new CSharpMethodParameter(addServiceCollection, "configure", configureArg, false, false));

            var dummyCode = new CSharpRepository();
            var ms = new MemoryStream();
            dllStream.CopyTo(ms);
            var a = Assembly.Load(ms.ToArray());
            foreach(var t in a.GetTypes())
            {
                if(!t.FullName.StartsWith($"{ns}.Services.")) { continue; }
                if(!t.IsInterface) { continue; }

                var sp = new CSharpInterface(sns, $"IServiceProvider", null);
                var gipc = new CSharpInterface(csns, $"IProxyConfig<{t.FullName}>", null);
                var gp = new CSharpInterface(csns, $"Proxy<{t.FullName}>", null);

                var cst = CSharpReflectionParser.AddType(dummyCode, t);

                var proxy = new CSharpClass(c, $"{t.FullName.Replace(".", "")}Proxy", null);
                proxy.AddMember(new CSharpModifier(c, "public"));
                proxy.AddExtends(gp);
                proxy.AddExtends(cst);
                c.AddMember(proxy);

                var ctr = new CSharpConstructor(proxy, "serviceProvider, config");
                var serviceProvider = new CSharpMethodParameter(ctr, "serviceProvider", sp, false, false);
                ctr.AddMember(serviceProvider);
                var proxyConfig = new CSharpMethodParameter(ctr, "config", gipc, false, false);
                ctr.AddMember(proxyConfig);
                proxy.AddMember(ctr);


                foreach (var p in cst.Properties)
                {
                    p.AddMember(new CSharpModifier(p, "public"));
                    proxy.AddMember(p);
                    p.Getter.AppendLine("throw new System.NotImplementedException();");
                }

                foreach (var m in cst.Methods)
                {
                    m.AddMember(new CSharpModifier(m, "public"));
                    proxy.AddMember(m);
                    if(m.ReturnType != null)
                    {
                        m.Body.Append($"return ");
                    }
                    m.Body.Append($"GetImplementation().{m.Name}(");
                    bool argEmitted = false;
                    foreach(var arg in m.Parameters)
                    {
                        if (argEmitted)
                        {
                            m.Body.Append(", ");
                        }
                        m.Body.Append(arg.Name);
                        argEmitted = true;
                    }
                    m.Body.AppendLine(");");
                }

                addServiceCollection.Body.AppendLine($"sc.SetupProxy<{t.FullName},{proxy.FullName}>(configure);");
            }

            addServiceCollection.Body.AppendLine("return sc;");
            c.AddMember(addServiceCollection);


        }
    }
}
