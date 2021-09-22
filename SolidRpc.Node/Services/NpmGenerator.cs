using ICSharpCode.SharpZipLib.Zip;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services.Code;
using SolidRpc.Abstractions.Types;
using SolidRpc.Abstractions.Types.Code;
using SolidRpc.Node.Services;
using SolidRpc.Node.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(INpmGenerator), typeof(NpmGenerator), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.Node.Services
{
    public class NpmGenerator : INpmGenerator
    {
        public NpmGenerator(ITypescriptGenerator typescriptGenerator, INodeService nodeService, ISerializerFactory serializerFactory)
        {
            TypescriptGenerator = typescriptGenerator;
            NodeService = nodeService;
            SerializerFactory = serializerFactory;
        }

        private ITypescriptGenerator TypescriptGenerator { get; }
        private INodeService NodeService { get; }
        private ISerializerFactory SerializerFactory { get; }

        public async Task<IEnumerable<NpmPackage>> CreateNpmPackage(IEnumerable<string> assemblyNames, CancellationToken cancellationToken = default)
        {
            if(assemblyNames.Any(o => o == "SolidRpcNode"))
            {
                assemblyNames = assemblyNames.Union(new[] { "SolidRpc.Abstractions" });
            }
            assemblyNames = assemblyNames.Union(new[] { "SolidRpcJs" });
            var ordinals = new List<string>() { "SolidRpcJs", "SolidRpc.Abstractions", "SolidRpcNode" };
            ordinals.AddRange(assemblyNames);
            var compileAssemblyNames = assemblyNames.OrderBy(o => ordinals.IndexOf(o));
            var npmPackages = new List<NpmPackage>();
            var npmPackagesToReturn = new HashSet<string>();
            foreach(var assemblyName in compileAssemblyNames)
            {
                var ts = await TypescriptGenerator.CreateTypesTsForAssemblyAsync(assemblyName, cancellationToken);
                var npmPackage = await CompileTsAsync(npmPackages, assemblyName, "1.0.0.0", ts, cancellationToken);
                npmPackages.Add(npmPackage);
                if(assemblyNames.Contains(assemblyName))
                {
                    npmPackagesToReturn.Add(npmPackage.Name);
                }
            }
            return npmPackages.Where(p => npmPackagesToReturn.Contains(p.Name));
        }

        private async Task<NpmPackage> CompileTsAsync(IEnumerable<NpmPackage> packages, string assemblyName, string version, string ts, CancellationToken cancellationToken)
        {
            var sep = Path.DirectorySeparatorChar;
            var inputFiles = packages.SelectMany(p => p.Files.Select(f => new { p.Name, File = f }))
                .Select(o => new NodeExecutionFile()
                {
                    FileName = $"node_modules{sep}{o.Name}{sep}{o.File.FilePath}",
                    Content = new MemoryStream(Encoding.UTF8.GetBytes(o.File.Content))
                }).Union(new[] {
                new NodeExecutionFile()
                {
                    FileName = $"index.ts",
                    Content = new MemoryStream(Encoding.UTF8.GetBytes(ts))
                }
            }).ToList();
            var nodeRes = await NodeService.ExecuteScriptAsync(new NodeExecutionInput()
            {
                Js = $"typescript{sep}lib{sep}tsc.js",
                Args = new string[0],
                InputFiles = inputFiles,
                ModuleId = NodeModuleRpcResolver.GuidModuleId
            }, cancellationToken);

            if (nodeRes.ExitCode != 0)
            {
                throw new Exception("NodeError:" + nodeRes.Out);
            }

            var packageName = assemblyName.ToLower().Replace('.', '-');
            var files = new List<NpmPackageFile>();
            foreach(var resFile in nodeRes.ResultFiles)
            {
                var content = TransformFileContent(packageName, version, packages.Select(o => o.Name), resFile.FileName, await ReadContent(resFile.Content));
                files.Add(new NpmPackageFile() { Content = content, FilePath = resFile.FileName });
            }
            return new NpmPackage()
            {
                Name = packageName,
                Files = files
            };
        }

        private string TransformFileContent(string packageName, string version, IEnumerable<string> packageNames, string fileName, string content)
        {
            if(fileName != "package.json")
            {
                return content;
            }
            SerializerFactory.DeserializeFromString(content, out NpmPackageJson pj);
            pj.Name = packageName;
            pj.Version = version;
            pj.Description = "";
            pj.Main = "index.js";
            pj.Types = "index.d.ts";
            pj.License = "ISC";
            packageNames.ToList().ForEach(o =>
            {
                pj.Dependencies[o] = $"file:../{o}";
            });
            SerializerFactory.SerializeToString(out content, pj);
            return content;
        }

        private async Task<string> ReadContent(Stream content)
        {
            using (var sr = new StreamReader(content))
            {
                return await sr.ReadToEndAsync();
            }
        }

        public async Task<FileContent> CreateInitialZip(CancellationToken cancellationToken = default)
        {
            var packages = await CreateNpmPackage(new[] { "SolidRpcNode" }, cancellationToken);

            // create package.json file
            var packageJson = new NpmPackageJson()
            {
                Name = "initial",
                Version = "1.0.0",
                Main = "index.js",
                Types = "index.d.js",
                Description = "",
                License = "ISC",
                Scripts = new Dictionary<string, string>()
                {
                    { "test", "echo \"Error: no test specified\" && exit 1" },
                    { "generate", "node generate.js && npm install" }
                }
            };

            packages.Select(o => o.Name).ToList().ForEach(o => {
                packageJson.Dependencies[o] = $"file:generated/{o}"; 
            });

            var ms = new MemoryStream();
            using (var zo = new ZipOutputStream(ms))
            {
                zo.SetLevel(9);
                SerializerFactory.SerializeToString(out string str, packageJson);
                await AddZipEntry(zo, "package.json", str);

                await AddZipEntry(zo, "generate.js", @"const sjs = require('solidrpcjs')
const snode = require('solidrpcnode')
sjs.SolidRpcJs.rootNamespace.setStringValue('baseUrl', 'http://localhost:7071/front/')
let packageNames = ['SolidRpcJs', 'SolidRpcNode', 'EO.BankId'];
snode.SolidRpcNode.addOAuth2PreFlightCallback();
snode.SolidRpcNode.generatePackages(packageNames); ");


                foreach (var package in packages)
                {
                    foreach (var f in package.Files)
                    {
                        await AddZipEntry(zo, $"generated/{package.Name}/{f.FilePath}", f.Content);
                    }
                }
            }
            return new FileContent()
            {
                Content = new MemoryStream(ms.ToArray()),
                ContentType = "application/zip"
            };
        }

        private Task AddZipEntry(ZipOutputStream zo, string fileName, string str)
        {
            var entry = new ZipEntry(fileName);
            zo.PutNextEntry(entry);
            var arr = Encoding.UTF8.GetBytes(str);
            return zo.WriteAsync(arr, 0, arr.Length);
        }
    }
}
