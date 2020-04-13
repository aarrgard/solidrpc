using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.NpmGenerator.InternalServices;
using SolidRpc.NpmGenerator.Types;

namespace SolidRpc.NpmGenerator.Services
{
    /// <summary>
    /// The npm generator implementation.
    /// </summary>
    public class NpmGenerator : INpmGenerator
    {
        private static ConcurrentDictionary<string, Task<byte[]>> cachedNpmPackages = new ConcurrentDictionary<string, Task<byte[]>>();

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="codeNamespaceGenerator"></param>
        /// <param name="typescriptGenerator"></param>
        /// <param name="serviceProvider"></param>
        public NpmGenerator(
            ILogger<NpmGenerator> logger,
            IHttpClientFactory httpClientFactory,
            IMethodBinderStore methodBinderStore,
            ICodeNamespaceGenerator codeNamespaceGenerator,
            ITypescriptGenerator typescriptGenerator,
            IInvoker<INpmGenerator> invoker,
            IServiceProvider serviceProvider)
        {
            Logger = logger;
            MethodBinderStore = methodBinderStore;
            CodeNamespaceGenerator = codeNamespaceGenerator;
            TypescriptGenerator = typescriptGenerator;
            HttpClientFactory = httpClientFactory;
            this.Invoker = invoker;
            ServiceProvider = serviceProvider;
        }

        private ILogger Logger { get; }
        private IMethodBinderStore MethodBinderStore { get; }
        private ICodeNamespaceGenerator CodeNamespaceGenerator { get; }
        private ITypescriptGenerator TypescriptGenerator { get; }
        private IHttpClientFactory HttpClientFactory { get; }
        private IInvoker<INpmGenerator> Invoker { get; }
        private IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Creates the npm tar
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FileContent> CreateNpm(string assemblyName, CancellationToken cancellationToken)
        {
            try
            {
                return new FileContent()
                {
                    FileName = $"{assemblyName}.tar.gz",
                    ContentType = "application/gzip",
                    Content = new MemoryStream(await cachedNpmPackages.GetOrAdd(assemblyName, CreateNpmInternal))
                };
            }
            catch
            {
                cachedNpmPackages.TryRemove(assemblyName, out Task<byte[]> cached);
                throw;
            }
        }

        private async Task<byte[]> CreateNpmInternal(string assemblyName)
        {
            Logger.LogInformation($"Creating npm package for assembly with name {assemblyName}");
            // create npm package
            var npmPackage = await CreateNpmPackage(assemblyName, CancellationToken.None);

            // verify package
            if(!npmPackage.Files.Any(o => o.FilePath == "package/dist/index.js"))
            {
                throw new Exception("package does not contain a index.js file in dist folder.");
            }
            byte[] arr;
            using (var outStream = new MemoryStream())
            {
                using (var gzoStream = new GZipOutputStream(outStream))
                using (var tarOutputStream = new TarOutputStream(gzoStream))
                {
                    foreach (var npmFile in npmPackage.Files)
                    {
                        var fileContent = Encoding.UTF8.GetBytes(npmFile.Content);
                        var tarEntry = TarEntry.CreateTarEntry(npmFile.FilePath);
                        tarEntry.Size = fileContent.Length;
                        tarOutputStream.PutNextEntry(tarEntry);
                        tarOutputStream.Write(fileContent, 0, fileContent.Length);
                        tarOutputStream.CloseEntry();
                    }
                }
                arr = outStream.ToArray();
            }
            return arr;
        }

        /// <summary>
        /// Creates an npm package.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<NpmPackage> CreateNpmPackage(string assemblyName, CancellationToken cancellationToken)
        {
            string packageName = "solidrpc";
            var packageVersion = string.Join(".", GetType().Assembly.GetName().Version.ToString().Split('.').Take(3));

            var assembly = MethodBinderStore.MethodBinders
                .Select(o => o.Assembly)
                .FirstOrDefault(o => o.GetName().Name == assemblyName);
            if(assembly != null)
            {
                packageName = assembly.GetName().Name.Replace('.', '-').ToLower();
                packageVersion = string.Join(".", assembly.GetName().Version.ToString().Split('.').Take(3));
            }

            //
            // create the package
            //
            var npmPackage = new NpmPackage();
            npmPackage.Files = new NpmPackageFile[]
            {
                new NpmPackageFile()
                {
                    FilePath = "package/src/types.ts",
                    Content =  await CreateTypesTs(assemblyName),
                },
                await CreatePackageJsonAsync(packageName, packageVersion,cancellationToken),
                CreateTsConfigJson(),
                CreateWebpackConfigJs()
            };

            //
            // compile the package
            //
            npmPackage = await RunNpm("all", npmPackage, cancellationToken);

            return npmPackage;
        }

        private async Task<NpmPackageFile> CreatePackageJsonAsync(string name, string version, CancellationToken cancellationToken)
        {
            var deps = new Dictionary<string, string>()
            {
                { "@types/qs", "^6.5.3"},
                { "qs", "^6.8.0"},
                { "axios", "^0.19.0"},
                { "rxjs", "^6.5.3"},
                { "cancellationtoken", "^2.0.1"},
            };
            if(!string.Equals(name,"solidrpc", StringComparison.InvariantCultureIgnoreCase))
            {
                var solidRpcNpmPackage = await Invoker.GetUriAsync(o => o.CreateNpm("SolidRpc", cancellationToken));
                deps["solidrpc"] = solidRpcNpmPackage.ToString();
            }
            return new NpmPackageFile()
            {
                FilePath = "package/package.json",
                Content = $@"{{
  ""name"": ""{name}"",
  ""version"": ""{version}"",
  ""main"": ""./dist/index.js"",
  ""types"": ""./src/types.d.ts"",
  ""dependencies"": {{
    {string.Join(",\r\n", deps.Select(o => $"\"{o.Key}\": \"{o.Value}\""))}
  }},
  ""devDependencies"": {{
    ""typescript"": ""^3.6.2"",
    ""@types/request"": ""^2.48.2"",
    ""webpack"": ""^4.39.3"",
    ""webpack-cli"": ""^3.3.8""
  }},
  ""scripts"": {{
    ""compile"": ""tsc"",
    ""webpack"": ""webpack --mode production"",
    ""all"": ""npm run compile && npm run webpack""
  }}
}}"
            };
        }

        private NpmPackageFile CreateTsConfigJson()
        {
            return new NpmPackageFile()
            {
                FilePath = "package/tsconfig.json",
                Content = $@"{{
  ""compilerOptions"": {{
    ""sourceMap"": true,
    ""declaration"": true,
    ""strictFunctionTypes"": true,
    ""noImplicitAny"": true,
    ""noImplicitReturns"": true,
    ""noImplicitThis"": true,
    ""moduleResolution"": ""node"",
    ""target"": ""es5""
  }},
  ""include"": [""./src/**/*.ts""]
}}"
            };
        }

        private NpmPackageFile CreateWebpackConfigJs()
        {
            return new NpmPackageFile()
            {
                FilePath = "package/webpack.config.js",
                Content = $@"const path = require('path');

module.exports = {{
    entry: ""./src/types.js"",
    output: {{
        path: path.resolve('dist'),
        filename: 'index.js',
        libraryTarget: 'commonjs2',
    }},
	resolve: {{
        extensions: ["".js""]
    }}
}};"
            };
        }

        /// <summary>
        /// Runs the supplied npm command on the supplied npm package
        /// </summary>
        /// <param name="npmCommmand"></param>
        /// <param name="npmPackage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<NpmPackage> RunNpm(string npmCommmand, NpmPackage npmPackage, CancellationToken cancellationToken = default(CancellationToken))
        {
            // explode supplied package in a 
            // new temp folder and compile there
            var tmpDir = ExtractNpmPackage(npmPackage);
            try
            {
                var npmFolder = await DownloadToCache("npm", "6.11.2");
                var script = WriteNodeServicesScript(tmpDir, "NpmGenerator.NpmScript.js");
                var nodeServicesOptions = new NodeServicesOptions(ServiceProvider);

                ////
                //// put old project path in the path env
                ////
                //var env = nodeServicesOptions.EnvironmentVariables;
                //if (env.TryGetValue("PATH", out string path))
                //{
                //    env["PATH"] = env["PATH"] + Path.PathSeparator + nodeServicesOptions.ProjectPath;
                //}
                //else
                //{
                //    env["PATH"] = nodeServicesOptions.ProjectPath;
                //}
                //nodeServicesOptions.EnvironmentVariables = env;

                nodeServicesOptions.ProjectPath = Path.Combine(tmpDir.FullName, "package");
                nodeServicesOptions.WatchFileExtensions = new string[0];
                nodeServicesOptions.InvocationTimeoutMilliseconds = 5 * 60 * 1000;
                using (var nodeService = NodeServicesFactory.CreateNodeServices(nodeServicesOptions))
                {
                    var resultCode = await nodeService.InvokeAsync<int>(
                        cancellationToken,
                        script.FullName,
                        npmFolder.FullName,
                        "install");
                    if (resultCode != 0)
                    {
                        throw new Exception("Failed to install npm packages");
                    }
                    resultCode = await nodeService.InvokeAsync<int>(
                        cancellationToken,
                        script.FullName,
                        npmFolder.FullName,
                        "run",
                        npmCommmand);
                    if (resultCode != 0)
                    {
                        throw new Exception("Failed to run command");
                    }
                }
                return CreateNpmPackage(tmpDir);
            }
            finally
            {
                for(int i = 0; i < 10; i++ )
                {
                    try
                    {
                        tmpDir.Delete(true);
                        break;
                    }
                    catch
                    {
                        await Task.Delay(1000);
                    }
                }
            }
        }

        private async Task<DirectoryInfo> DownloadToCache(string moduleName, string version)
        {
            var cachedModuleDir = new DirectoryInfo(Path.Combine(Path.GetTempPath(), "node_modules", moduleName, version));
            if (!cachedModuleDir.Exists)
            {
                cachedModuleDir.Create();
                await DownloadNodeModule(cachedModuleDir, moduleName, version);
            }
            var allFilesExists = CheckCachedModuleDir(cachedModuleDir);
            if(!allFilesExists)
            {
                cachedModuleDir.Delete(true);
                await DownloadNodeModule(cachedModuleDir, moduleName, version);
            }
            return cachedModuleDir;
        }

        /// <summary>
        /// Returns true if all the files exists(temp dir might get corrupted).
        /// </summary>
        /// <param name="cachedModuleDir"></param>
        /// <returns></returns>
        private bool CheckCachedModuleDir(DirectoryInfo cachedModuleDir)
        {
            var filesFile = new FileInfo(Path.Combine(cachedModuleDir.Parent.FullName, $"{cachedModuleDir.Name}.files"));
            if(!filesFile.Exists)
            {
                return false;
            }
            using (var fos = filesFile.OpenText())
            {
                var line = fos.ReadLine();
                while(line != null)
                {
                    line = fos.ReadLine();
                }
            }
            return true;
        }

        private async Task DownloadNodeModule(DirectoryInfo tmpDir, string moduleName, string version)
        {
            Logger.LogInformation($"Extracting {moduleName}-{version} to {tmpDir.FullName}");
            var url = $"https://registry.npmjs.org/{moduleName}/-/{moduleName}-{version}.tgz";
            var httpClient = HttpClientFactory.CreateClient();
            var resp = await httpClient.GetAsync(url);
            var extractedFiles = new List<string>();
            using (var s = await resp.Content.ReadAsStreamAsync())
            {
                using (var gzi = new GZipInputStream(s))
                {
                    using (var tis = new TarInputStream(gzi))
                    {
                        var nextEntry = tis.GetNextEntry();
                        while(nextEntry != null)
                        {
                            using (var fos = CreateOutputStream(tmpDir, nextEntry, extractedFiles))
                            {
                                tis.CopyEntryContents(fos);
                            }
                            nextEntry = tis.GetNextEntry();
                        }

                    }
                }
            }

            // put a file in parent directory containing all the files
            var filesFile = new FileInfo(Path.Combine(tmpDir.Parent.FullName, $"{tmpDir.Name}.files"));
            using (var fos = filesFile.CreateText())
            {
                extractedFiles.ToList().ForEach(o => fos.WriteLine(o));
            }
        }

        private Stream CreateOutputStream(DirectoryInfo moduleDir, TarEntry nextEntry, ICollection<string> extractedFiles)
        {
            var fileName = nextEntry.Name;
            if(fileName.StartsWith("package/"))
            {
                fileName = fileName.Substring("package/".Length);
            }
            extractedFiles.Add(fileName);
            fileName = fileName.Replace('/', Path.DirectorySeparatorChar);
            var fi = new FileInfo(Path.Combine(moduleDir.FullName, fileName));
            if(!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            return fi.Create();
        }

        private FileInfo WriteNodeServicesScript(DirectoryInfo tmpDir, string scriptName)
        {
            var compilerScriptResourceName = GetType().Assembly
                .GetManifestResourceNames()
                .Where(o => o.EndsWith(scriptName, StringComparison.InvariantCultureIgnoreCase))
                .Single();
            var fi = new FileInfo(Path.Combine(tmpDir.FullName, "npm.js"));
            using (var tw = fi.CreateText())
            using (var s = GetType().Assembly.GetManifestResourceStream(compilerScriptResourceName))
            using (var sw = new StreamReader(s))
            {
                tw.Write(sw.ReadToEnd());
            }
            return fi;
        }

        private NpmPackage CreateNpmPackage(DirectoryInfo dir)
        {
            var npmPackage = new NpmPackage();
            CreateNpmPackage(npmPackage, dir, dir);
            npmPackage.Files = npmPackage.Files.ToList();
            return npmPackage;
        }

        private void CreateNpmPackage(NpmPackage npmPackage, DirectoryInfo baseDir, DirectoryInfo currentDir)
        {
            if (currentDir.FullName == Path.Combine(baseDir.FullName, "package", "node_modules"))
            {
                return;
            }
            if (currentDir.FullName == Path.Combine(baseDir.FullName, "npm.js"))
            {
                return;
            }
            foreach (var d in currentDir.GetDirectories())
            {
                CreateNpmPackage(npmPackage, baseDir, d);
            }
            npmPackage.Files = (npmPackage.Files ?? new NpmPackageFile[0])
                .Union(currentDir.GetFiles().Select(o => CreateNpmPackageFile(baseDir, o)));
        }

        private NpmPackageFile CreateNpmPackageFile(DirectoryInfo baseDir, FileInfo fi)
        {
            var filePath = fi.FullName.Substring(baseDir.FullName.Length);
            filePath = string.Join("/", filePath.Split(Path.DirectorySeparatorChar).Where(o => !string.IsNullOrEmpty(o)));
            using (var tr = fi.OpenText())
            {
                return new NpmPackageFile()
                {
                    Content = tr.ReadToEnd(),
                    FilePath = filePath
                };
            }
        }

        private DirectoryInfo ExtractNpmPackage(NpmPackage npmPackage)
        {
            var tmpPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var tmpDir = new DirectoryInfo(tmpPath);
            tmpDir.Create();
            foreach(var packageFile in npmPackage.Files)
            {
                ExtractPackageFile(tmpDir, packageFile.FilePath, packageFile.Content);
            }
            return tmpDir;
        }

        private void ExtractPackageFile(DirectoryInfo dir, string filePath, string content)
        {
            var pathItems = filePath.Split('/');
            if(pathItems.Length > 1)
            {
                var subDir = new DirectoryInfo(Path.Combine(dir.FullName, pathItems.First()));
                if(!subDir.Exists)
                {
                    subDir.Create();
                }
                ExtractPackageFile(subDir, string.Join("/", pathItems.Skip(1)), content);
                return;
            }
            else
            {
                var fi = new FileInfo(Path.Combine(dir.FullName, pathItems.First()));
                using (var tw = fi.CreateText())
                {
                    tw.Write(content);
                } 
            }
        }

        /// <summary>
        /// Creates a code representation for supplied assembly
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<CodeNamespace> CreateCodeNamespace(string assemblyName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(CodeNamespaceGenerator.CreateCodeNamespace(assemblyName));
        }

        /// <summary>
        /// Creates the types.ts file for supplied assembly
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> CreateTypesTs(string assemblyName, CancellationToken cancellationToken = default(CancellationToken))
        {
            //
            // create static package
            //
            var resName = GetType().Assembly.GetManifestResourceNames().FirstOrDefault(o => o.EndsWith($".{assemblyName}.ts", StringComparison.InvariantCultureIgnoreCase));
            if(resName != null)
            {
                using (var s = GetType().Assembly.GetManifestResourceStream(resName))
                using (var sr = new StreamReader(s))
                {
                    return sr.ReadToEnd();
                }
            }

            //
            // create dynamoc package
            //
            var codeNamespace = await CreateCodeNamespace(assemblyName);
            return TypescriptGenerator.CreateTypesTs(codeNamespace);
        }
    }
}
