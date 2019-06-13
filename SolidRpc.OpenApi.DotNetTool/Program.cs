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

            Task task;
            switch(command)
            {
                case s_code2openapi:
                    task = GenerateOpenApiFromCode(files);
                    break;
                case s_openapi2code:
                    task = GenerateCodeFromSwagger(files);
                    break;
                default:
                    throw new Exception("Cannot handle command:" + command);
            }
            task.GetAwaiter().GetResult();
        }

        private static Task GenerateOpenApiFromCode(IEnumerable<FileInfo> files)
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
            return GenerateOpenApiFromCode(files.First());
        }

        private static async Task GenerateOpenApiFromCode(FileInfo fileInfo)
        {
            var sp = GetServiceProvider();
            var gen = sp.GetRequiredService<IOpenApiGenerator>();
            var projectZip = await new DirectoryInfo(Directory.GetCurrentDirectory()).CreateFileDataZip();
            var project = await gen.ParseProjectZip(projectZip);
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

        private static Task GenerateCodeFromSwagger(List<FileInfo> files)
        {
            var nonexisingFiles = files.Where(o => !o.Exists);
            if (nonexisingFiles.Any())
            {
                Console.Error.WriteLine($"Cannot find files {string.Join(",", nonexisingFiles.Select(o => o.FullName))}");
                Environment.Exit(1);
            }
            return Task.WhenAll(files.Select(o => GenerateCodeFromOpenApi(o)));
        }

        private static async Task GenerateCodeFromOpenApi(FileInfo openApiFile)
        {
            var sp = GetServiceProvider();
            var gen = sp.GetRequiredService<IOpenApiGenerator>();

            var projectDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            var projectZip = await projectDir.CreateFileDataZip();
            var project = await gen.ParseProjectZip(projectZip);
            var csproj = project.ProjectFiles
                .Where(o => o.Directory == "")
                .Where(o => o.FileData.Filename.EndsWith(".csproj"))
                .SingleOrDefault();

            if (csproj == null)
            {
                throw new Exception("Cannot find csproj file in project.");
            }

            var settings = await gen.GetSettingsCodeGenFromCsproj(csproj.FileData);

            using (var fr = openApiFile.OpenText())
            {
                settings.SwaggerSpec = fr.ReadToEnd();
            }
            project = await gen.CreateCodeFromOpenApiSpec(settings);
            projectZip = await gen.CreateProjectZip(project);
            await projectDir.WriteFileDataZip(projectZip);
        }
    }
}
