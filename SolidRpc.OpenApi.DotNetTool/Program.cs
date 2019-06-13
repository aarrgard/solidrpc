using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.OpenApi.Generator;
using SolidRpc.OpenApi.Generator.Impl.Services;
using SolidRpc.OpenApi.Generator.Services;
using SolidRpc.OpenApi.Generator.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace SolidRpc.OpenApi.DotNetTool
{
    class Program
    {
        private const string s_openapi2code = "-openapi2code";
        private const string s_code2openapi = "-code2openapi";
        private static string[] s_commands = new[] { s_openapi2code, s_code2openapi };
        private static ConcurrentDictionary<string, XmlDocument> ProjectDocuments = new ConcurrentDictionary<string, XmlDocument>();
        private static IServiceProvider s_serviceProvider;

        static void Main(string[] args)
        {
            Console.WriteLine("Running swagger-generator");
            var argList = new List<string>(args);
            var command = argList.Where(o => s_commands.Contains(o)).SingleOrDefault();
            if(command == null)
            {
                Console.Error.WriteLine($"Must supply direction of code: {string.Join("or", s_commands)}");
                Environment.Exit(1);
            }
            argList.Remove(command);

            var workingDir = Directory.GetCurrentDirectory();
            var files = argList.Select(o => new FileInfo(Path.Combine(workingDir, o))).ToList();

            switch(command)
            {
                case s_code2openapi:
                    GenerateOpenApiFromCode(files);
                    break;
                case s_openapi2code:
                    GenerateCodeFromSwagger(files);
                    break;
            }
        }

        private static void GenerateOpenApiFromCode(IEnumerable<FileInfo> files)
        {
            if (files.Count() == 0)
            {
                Console.Error.WriteLine($"Must specify file to generate");
                Environment.Exit(1);
            }
            if (files.Count() > 1)
            {
                Console.Error.WriteLine($"Cannot specify more than one file when generating openapi from code.");
                Environment.Exit(1);
            }
            GenerateOpenApiFromCode(files.First()).GetAwaiter().GetResult();
        }

        private static async Task GenerateOpenApiFromCode(FileInfo fileInfo)
        {
            var sp = GetServiceProvider();
            var gen = sp.GetRequiredService<IOpenApiGenerator>();
            var projectZip = await CreateZip();
            var project = await gen.ParseProject(projectZip);
            var csproj = project.ProjectFiles
                .Where(o => o.Directory == "")
                .Where(o => o.FileData.Filename.EndsWith(".csproj"))
                .SingleOrDefault();

            if(csproj == null)
            {
                throw new Exception("Cannot find csproj file in project.");
            }

            var settings = await gen.GetSettingsSpecGenFromCsproj(csproj.FileData);
            var spec = await gen.CreateOpenApiSpecFromCode(settings, project);
            using (var fs = fileInfo.Create())
            {
                await spec.FileStream.CopyToAsync(fs);
            }
        }

        private static async Task<FileData> CreateZip()
        {
            var dir = Directory.GetCurrentDirectory();

            var ms = new MemoryStream();
            using (var zipStream = new ZipOutputStream(ms))
            {
                await CreateZip(new DirectoryInfo(dir), "/", zipStream);
            }

            return new FileData()
            {
                ContentType = "application/zip",
                Filename = "project.zip",
                FileStream = new MemoryStream(ms.ToArray())
            };
        }

        private static async Task CreateZip(DirectoryInfo dir, string folder, ZipOutputStream zipStream)
        {
            var extensions = new[]
            {
                ".cs", ".csproj"
            };
            foreach (var subDir in dir.GetDirectories())
            {
                await CreateZip(subDir, $"{folder}{subDir.Name}/", zipStream);
            }
            foreach (var file in dir.GetFiles())
            {
                if (!extensions.Any(o => string.Equals(file.Extension, o, StringComparison.InvariantCultureIgnoreCase)))
                {
                    continue;
                }
                var entry = new ZipEntry($"{folder}{file.Name}");
                zipStream.PutNextEntry(entry);
                try
                {
                    using (var fs = file.OpenRead())
                    {
                        await fs.CopyToAsync(zipStream);
                    }
                }
                finally
                {
                    zipStream.CloseEntry();
                }

            }
        }

        private static IServiceProvider GetServiceProvider()
        {
            if(s_serviceProvider == null)
            {
                var sc = new ServiceCollection();
                sc.AddTransient<IOpenApiGenerator, OpenApiGenerator>();
                s_serviceProvider = sc.BuildServiceProvider();
            }
            return s_serviceProvider;
        }

        private static void GenerateCodeFromSwagger(List<FileInfo> files)
        {
            var nonexisingFiles = files.Where(o => !o.Exists);
            if (nonexisingFiles.Any())
            {
                Console.Error.WriteLine($"Cannot find files {string.Join(",", nonexisingFiles.Select(o => o.FullName))}");
                Environment.Exit(1);
            }
            files.ForEach(o => GenerateCodeFromSwagger(o));
        }

        private static void GenerateCodeFromSwagger(FileInfo swaggerFile)
        {
            using (var fr = swaggerFile.OpenText())
            {
                var swaggerSpec = fr.ReadToEnd();
                var settings = new SettingsCodeGen()
                {
                    SwaggerSpec = swaggerSpec,
                    ProjectNamespace = GetProjectNamespace()
                };

                OpenApiCodeGenerator.GenerateCode(settings);
            }
        }

        private static FileInfo GetCsProjFile()
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            // locate csproj file
            var csprojFiles = dir.GetFiles("*.csproj");
            if (csprojFiles.Length != 1)
            {
                throw new Exception("Cannot find csproj file in folder:" + dir.FullName);
            }
            return csprojFiles.First();
        }

        private static string GetProjectNamespace()
        {
            var csProjFile = GetCsProjFile();
            var projectNamespace = csProjFile.Name;
            projectNamespace = projectNamespace.Substring(0, projectNamespace.Length - csProjFile.Extension.Length);
            return projectNamespace;
        }

        private static string GetAssemblyName()
        {
            var csProjFile = GetCsProjFile();
            var assemblyName = csProjFile.Name;
            assemblyName = assemblyName.Substring(0, assemblyName.Length - csProjFile.Extension.Length);
            return assemblyName;
        }
    }
}
