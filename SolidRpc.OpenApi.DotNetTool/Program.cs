using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.OpenApi.Generator;
using SolidRpc.OpenApi.Generator.Impl.Services;
using SolidRpc.OpenApi.Generator.Services;
using SolidRpc.OpenApi.Generator.Types;
using SolidRpc.OpenApi.Model;
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
    public class Program
    {
        private const string s_openapi2code = "-openapi2code";
        private const string s_code2openapi = "-code2openapi";
        private static string[] s_commands = new[] { s_openapi2code, s_code2openapi };
        private static readonly ConcurrentDictionary<string, XmlDocument> ProjectDocuments = new ConcurrentDictionary<string, XmlDocument>();
        private static IServiceProvider s_serviceProvider;

        public static void Main(string[] args)
        {
            try
            {
                MainWithExeptions(args).GetAwaiter().GetResult();
            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }
        public static Task MainWithExeptions(string[] args)
        {
            Console.WriteLine("Running dotnet-openapigen");
            var argList = new List<string>(args);
            var command = argList.Where(o => s_commands.Contains(o)).SingleOrDefault();
            if(command == null)
            {
                Console.Error.WriteLine($"Must supply direction of code: {string.Join("or", s_commands)}");
                Environment.Exit(1);
            }
            argList.Remove(command);

            var workingDir = new DirectoryInfo(GetArgParam(argList, "-d", Directory.GetCurrentDirectory()));
            var settings = GetSettings(argList);
            var files = argList.Select(o => new FileInfo(Path.Combine(workingDir.FullName, o))).ToList();

            Task task;
            switch(command)
            {
                case s_code2openapi:
                    task = GenerateOpenApiFromCode(workingDir, settings, files);
                    break;
                case s_openapi2code:
                    task = GenerateCodeFromOpenApi(workingDir, settings, files);
                    break;
                default:
                    throw new Exception("Cannot handle command:" + command);
            }

            return task;
        }

        private static IDictionary<string, string> GetSettings(List<string> argList)
        {
            var dict = new Dictionary<string, string>();
            for (int i = 0; i < argList.Count; i++)
            {
                if (argList[i].StartsWith("-"))
                {
                    dict[argList[i].Substring(1)] = argList[i + 1];
                    argList.RemoveAt(i);
                    argList.RemoveAt(i);
                    i--;
                }
            }
            return dict;
        }

        private static string GetArgParam(List<string> argList, string arg, string defaultValue)
        {
            var argIdx = argList.IndexOf(arg);
            if(argIdx == -1)
            {
                return defaultValue;
            }
            var val = argList[argIdx + 1];
            argList.RemoveAt(argIdx);
            argList.RemoveAt(argIdx);
            return val;
        }

        private static Task GenerateOpenApiFromCode(DirectoryInfo workingDir, IDictionary<string, string> settings, IEnumerable<FileInfo> files)
        {
            if (files.Count() == 0)
            {
                throw new Exception($"Must specify file to generate");
            }
            if (files.Count() > 1)
            {
                throw new Exception($"Cannot specify more than one file when generating openapi from code.");
            }
            return GenerateOpenApiFromCode(workingDir, settings, files.First());
        }

        private static async Task GenerateOpenApiFromCode(DirectoryInfo workingDir, IDictionary<string, string> argSettings, FileInfo fileInfo)
        {
            var sp = GetServiceProvider();
            var gen = sp.GetRequiredService<IOpenApiGenerator>();
            var projectZip = await workingDir.CreateFileDataZip();
            var project = await gen.ParseProjectZip(projectZip);
            var csproj = project.ProjectFiles
                .Where(o => o.Directory == "")
                .Where(o => o.FileData.Filename.EndsWith(".csproj"))
                .SingleOrDefault();

            var settings = new SettingsSpecGen()
            {
                Title = workingDir.Name,
                Version = "1.0.0"
            };
            if (csproj != null)
            {
                settings = await gen.GetSettingsSpecGenFromCsproj(csproj.FileData);
            }
            UpdateSettingsFromArguments(argSettings, settings);

            var spec = await gen.CreateOpenApiSpecFromCode(settings, project);
            if (argSettings.TryGetValue("only-compare", out string onlyCompare) && onlyCompare.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                using (var fs = fileInfo.OpenRead())
                {
                    CompareStreams(fileInfo.FullName, spec.FileStream, fs);
                }
            }
            else
            {
                using (var fs = fileInfo.Create())
                {
                    await spec.FileStream.CopyToAsync(fs);
                }
            }
        }

        private static void CompareStreams(string fileName,Stream s1, FileStream s2)
        {
            while(true)
            {
                var b1 = s1.ReadByte();
                var b2 = s2.ReadByte();
                if(b1 != b2)
                {
                    throw new Exception($"File {fileName} differs");
                }
                if(b1 == -1)
                {
                    return;
                }
            }
        }

        private static IServiceProvider GetServiceProvider()
        {
            if(s_serviceProvider == null)
            {
                var sc = new ServiceCollection();
                sc.AddSingleton<IOpenApiParser, OpenApiParser>();
                sc.AddTransient<IOpenApiGenerator, OpenApiGenerator>();
                s_serviceProvider = sc.BuildServiceProvider();
            }
            return s_serviceProvider;
        }

        private static Task GenerateCodeFromOpenApi(DirectoryInfo workingDir, IDictionary<string, string> settings, List<FileInfo> files)
        {
            var nonexisingFiles = files.Where(o => !o.Exists);
            if (nonexisingFiles.Any())
            {
                throw new Exception($"Cannot find files {string.Join(",", nonexisingFiles.Select(o => o.FullName))}");
            }
            return Task.WhenAll(files.Select(o => GenerateCodeFromOpenApi(workingDir, settings, o)));
        }

        private static async Task GenerateCodeFromOpenApi(DirectoryInfo projectDir, IDictionary<string, string> argSettings, FileInfo openApiFile)
        {
            var sp = GetServiceProvider();
            var gen = sp.GetRequiredService<IOpenApiGenerator>();

            var projectZip = await projectDir.CreateFileDataZip();
            var project = await gen.ParseProjectZip(projectZip);
            var csproj = project.ProjectFiles
                .Where(o => o.Directory == "")
                .Where(o => o.FileData.Filename.EndsWith(".csproj"))
                .SingleOrDefault();

            var settings = new SettingsCodeGen();
            if (csproj != null)
            {
                settings = await gen.GetSettingsCodeGenFromCsproj(csproj.FileData);
            }
            UpdateSettingsFromArguments(argSettings, settings);

            using (var fr = openApiFile.OpenText())
            {
                settings.SwaggerSpec = fr.ReadToEnd();
            }
            project = await gen.CreateCodeFromOpenApiSpec(settings);
            projectZip = await gen.CreateProjectZip(project);

            if(argSettings.ContainsKey("only-compare") && bool.Parse(argSettings["only-compare"]))
            {
                if(await projectDir.FileDataZipDiffers(projectZip))
                {
                    throw new Exception("File differs!");
                }
            }
            else
            {
                await projectDir.WriteFileDataZip(projectZip);
            }
        }

        private static void UpdateSettingsFromArguments(IDictionary<string, string> argSettings, SettingsGen settings)
        {
            foreach(var prop in settings.GetType().GetProperties())
            {
                if (!prop.CanRead) continue;
                if (!prop.CanWrite) continue;
                if(argSettings.TryGetValue(prop.Name, out string val))
                {
                    prop.SetValue(settings, val);
                }
            }
        }
    }
}
