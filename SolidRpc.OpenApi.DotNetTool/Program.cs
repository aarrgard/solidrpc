using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.OpenApi.Generator.Impl.Services;
using SolidRpc.OpenApi.Generator.Services;
using SolidRpc.OpenApi.Generator.Types;
using SolidRpc.OpenApi.Generator.Types.Project;
using SolidRpc.OpenApi.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.DotNetTool
{
    public class Program
    {
        private const string s_openapi2code = "-openapi2code";
        private const string s_code2openapi = "-code2openapi";
        private const string s_serverbindings = "-serverbindings";
        private static string[] s_commands = new[] { s_openapi2code, s_code2openapi, s_serverbindings };
        private static IServiceProvider s_serviceProvider;
        /// <summary>
        /// 
        /// </summary>
        public static DirectoryInfo GetProjectFolder(string projectName)
        {
            var dir = new DirectoryInfo(".");
            while (dir.Parent != null)
            {
                if (dir.Parent.Name == projectName)
                {
                    return dir.Parent;
                }
                var childeFolder = dir.GetDirectories().Where(x => x.Name == projectName).FirstOrDefault();
                if (childeFolder != null)
                {
                    return childeFolder;
                }
                dir = dir.Parent;
            }
            throw new Exception("Cannot find project folder:" + projectName);
        }

        /// <summary>
        /// Returns the file in supplied dir or one of the sub directories
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileInfo FindFile(DirectoryInfo directoryInfo, string fileName)
        {
            var fileInfo = directoryInfo.GetFiles().Where(o => o.Name == fileName).FirstOrDefault();
            if (fileInfo != null)
            {
                return fileInfo;
            }
            foreach (var subDir in directoryInfo.GetDirectories())
            {
                fileInfo = FindFile(subDir, fileName);
                if (fileInfo != null)
                {
                    return fileInfo;
                }
            }
            return null;
        }

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
                Console.Error.WriteLine($"Must supply a command: {string.Join(" or ", s_commands)}");
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
                case s_serverbindings:
                    task = GenerateServerBindings(workingDir, settings, files);
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

        private static Task GenerateServerBindings(DirectoryInfo projectDir, IDictionary<string, string> settings, IEnumerable<FileInfo> files)
        {
            if (files.Count() == 0)
            {
                throw new Exception($"Must specify file to generate");
            }
            if (files.Count() > 1)
            {
                throw new Exception($"Cannot specify more than one file when generating server bindings.");
            }
            return GenerateServerBindings(projectDir, settings, files.First());
        }

        private static async Task GenerateServerBindings(DirectoryInfo projectDir, IDictionary<string, string> argSettings, FileInfo fileInfo)
        {
            var sp = GetServiceProvider();
            var gen = sp.GetRequiredService<IOpenApiGenerator>();
            var projectZip = await projectDir.CreateFileDataZip();
            var project = await gen.ParseProjectZip(projectZip);

            var settings = new SettingsServerGen() { };
            UpdateSettingsFromArguments(argSettings, settings);

            var packagesPath = project?.Assets?.Project?.Restore?.PackagesPath;
            if (string.IsNullOrEmpty(packagesPath))
            {
                throw new Exception("Cannot located the packages path from the project.assets.json");
            }
            using (var tw = fileInfo.CreateText())
            {
                var dlls = new List<FileInfo>();
                foreach (var targetLib in nn(project.Assets?.Targets?.First().Value))
                {
                    if(!project.Assets.Libraries.TryGetValue(targetLib.Key, out ProjectAssetsLibrary lib))
                    {
                        throw new Exception("Cannot locate the library " + targetLib.Key);
                    }
                    if (targetLib.Value.Compile == null)
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(lib.Path))
                    {
                        continue;
                    }
                    if (lib.Type == "package")
                    {
                        var nugetLocation = Path.Combine(packagesPath, lib.Path);
                        nugetLocation = Path.Combine(nugetLocation, targetLib.Value.Compile.First().Key);
                        var dllLocation = new FileInfo(nugetLocation);
                        if (dllLocation.Exists)
                        {
                            tw.WriteLine($"//{targetLib.Key}=>{dllLocation.FullName}");
                            dlls.Add(dllLocation);
                        }
                        else
                        {
                            tw.WriteLine($"//{targetLib.Key}=>");
                        }
                    }
                    else if (lib.Type == "project")
                    {
                        var projectPath = projectDir.FullName;
                        if (!string.IsNullOrEmpty(settings.ProjectBaseFix))
                        {
                            projectPath = Path.Combine(projectPath, settings.ProjectBaseFix);
                        }
                        var csProjFile = new FileInfo(Path.Combine(projectPath, lib.Path));
                        if(!csProjFile.Exists)
                        {
                            tw.WriteLine($"//{targetLib.Key}=>!!!!{csProjFile.FullName}");
                        }
                        var dir = csProjFile.Directory;
                        var segments = targetLib.Value.Compile.First().Key.Split('/');
                        var dllLocation = LocateFile(segments, csProjFile.Directory);
                        if (dllLocation?.Exists ?? false)
                        {
                            tw.WriteLine($"//{targetLib.Key}=>{dllLocation.FullName}");
                            dlls.Add(dllLocation);
                        }
                        else
                        {
                            tw.WriteLine($"//{targetLib.Key}=>not:{string.Join("/",segments)}");
                        }
                    }
                    else
                    {
                        throw new Exception("Cannot handle library type: " + lib.Type);
                    }
                }

                dlls = GetSolidRpcDlls(dlls).ToList();

                foreach(var dll in dlls)
                {
                    tw.WriteLine("//Generating stubs for : " + dll.FullName);
                }

                project = new Project()
                {
                    Files = dlls.Select(o => CreateFile(o)).ToList()
                };

                project = await gen.CreateServerCode(settings, project);
                projectZip = await gen.CreateProjectZip(project);

                if (argSettings.ContainsKey("only-compare") && bool.Parse(argSettings["only-compare"]))
                {
                    var filesThatDiffers = await projectDir.FileDataZipDiffers(projectZip);
                    if (filesThatDiffers.Count > 0)
                    {
                        throw new Exception($"Files differs({string.Join(",", filesThatDiffers)})!");
                    }
                }
                else
                {
                    await projectDir.WriteFileDataZip(projectZip);
                }
            }
        }

        private static ProjectFile CreateFile(FileInfo fi)
        {
            var ms = new MemoryStream();
            using (var fs = fi.OpenRead())
            {
                fs.CopyTo(ms);
            }
            ms.Position = 0;
            return new ProjectFile()
            {
                FileData = new FileData()
                {
                    Filename = fi.Name,
                    ContentType = "application/octet-stream",
                    FileStream = ms
                },
                Directory = ""
            };
        }

        private static IEnumerable<FileInfo> GetSolidRpcDlls(IEnumerable<FileInfo> dlls)
        {
            var solidRpcDlls = new List<FileInfo>();
            foreach (var dll in dlls)
            {
                try
                {
                    using (var fs = dll.OpenRead())
                    {
                        using var peReader = new PEReader(fs);
                        MetadataReader mr = peReader.GetMetadataReader();

                        var md = mr.GetModuleDefinition();
                        var moduleName = mr.GetString(md.Name);
                        if(moduleName.EndsWith(".dll"))
                        {
                            moduleName = moduleName.Substring(0, moduleName.Length - 4);
                        }

                        foreach (var res in mr.ManifestResources)
                        {
                            var x = mr.GetManifestResource(res);
                            var name = mr.GetString(x.Name);
                            if(name == $"{moduleName}.{moduleName}.json")
                            {
                                solidRpcDlls.Add(dll);
                            }
                        }
                    }
                }
                catch { }
            }

            return solidRpcDlls;
        }

        private static FileInfo LocateFile(IEnumerable<string> segments, DirectoryInfo directory)
        {
            if(directory == null)
            {
                return null;
            }
            if(segments.Count() == 1)
            {
                return directory.GetFiles().FirstOrDefault(o => string.Equals(o.Name, segments.First(), StringComparison.InvariantCulture));
            }
            if(segments.First() == "placeholder")
            {
                foreach(var dir in directory.GetDirectories())
                {
                    var fileInfo = LocateFile(segments.Skip(1), dir);
                    if (fileInfo == null)
                    {
                        fileInfo = LocateFile(segments, dir);
                    }
                    if (fileInfo != null)
                    {
                        return fileInfo;
                    }

                }
            }
            return LocateFile(segments.Skip(1), directory.GetDirectories().FirstOrDefault(o => string.Equals(o.Name, segments.First(), StringComparison.InvariantCulture)));
        }

        private static IDictionary<string, T> nn<T>(IDictionary<string, T> x)
        {
            return x ?? new Dictionary<string, T>();
        }

        private static IEnumerable<T> nn<T>(IEnumerable<T> x)
        {
            return x ?? new T[0];
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
            var csprojs = project.Files
                .Where(o => o.Directory == "")
                .Where(o => o.FileData.Filename.EndsWith(".csproj"));
            if (csprojs.Count() > 1) throw new Exception($"Directory {workingDir} contains more than one .csproj file");
            var csproj = csprojs.SingleOrDefault();


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
                sc.AddSolidRpcSingletonServices();
                sc.AddSingleton<IOpenApiSpecResolver, OpenApiSpecResolverFile>();
                sc.AddTransient<IOpenApiGenerator, OpenApiGenerator>();
                s_serviceProvider = sc.BuildServiceProvider();
            }
            return s_serviceProvider;
        }

        private static Task GenereateServerBindings(DirectoryInfo workingDir, IDictionary<string, string> settings, List<FileInfo> files)
        {
            var nonexisingFiles = files.Where(o => !o.Exists);
            if (nonexisingFiles.Any())
            {
                throw new Exception($"Cannot find files {string.Join(",", nonexisingFiles.Select(o => o.FullName))}");
            }
            return Task.WhenAll(files.Select(o => GenerateCodeFromOpenApi(workingDir, settings, o)));
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
            var csproj = project.Files
                .Where(o => o.Directory == "")
                .Where(o => o.FileData.Filename.EndsWith(".csproj"))
                .SingleOrDefault();

            var settings = new SettingsCodeGen();
            if (csproj != null)
            {
                settings = await gen.GetSettingsCodeGenFromCsproj(csproj.FileData);
            }
            UpdateSettingsFromArguments(argSettings, settings);
            settings.SwaggerSpecFile = MakeRelativePath(projectDir.FullName + Path.DirectorySeparatorChar, openApiFile.FullName).Replace('\\', '/');

            project = await gen.CreateCodeFromOpenApiSpec(settings, project);
            projectZip = await gen.CreateProjectZip(project);

            if(argSettings.ContainsKey("only-compare") && bool.Parse(argSettings["only-compare"]))
            {
                var filesThatDiffers = await projectDir.FileDataZipDiffers(projectZip);
                if (filesThatDiffers.Count > 0)
                {
                    throw new Exception($"Files differs({string.Join(",", filesThatDiffers)})!");
                }
            }
            else
            {
                await projectDir.WriteFileDataZip(projectZip);
            }
        }
        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path or <c>toPath</c> if the paths are not related.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static String MakeRelativePath(String fromPath, String toPath)
        {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
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
