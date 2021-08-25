using ICSharpCode.SharpZipLib.Zip;
using SolidRpc.Abstractions;
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
        public NpmGenerator(ITypescriptGenerator typescriptGenerator, INodeService nodeService)
        {
            TypescriptGenerator = typescriptGenerator;
            NodeService = nodeService;
        }

        private ITypescriptGenerator TypescriptGenerator { get; }
        private INodeService NodeService { get; }

        public async Task<IEnumerable<NpmPackage>> CreateNpmPackage(IEnumerable<string> assemblyNames, CancellationToken cancellationToken = default)
        {
            var compileAssemblyNames = assemblyNames.Union(new[] { "SolidRpc" }).Distinct().OrderBy(o => o == "SolidRpc" ? 0 : 1);
            var npmPackages = new List<NpmPackage>();
            foreach(var assemblyName in compileAssemblyNames)
            {
                var ts = await TypescriptGenerator.CreateTypesTsForAssemblyAsync(assemblyName);
                var npmPackage = await CompileTsAsync(npmPackages, assemblyName, ts);
                npmPackages.Add(npmPackage);
            }
            return npmPackages.Where(p => assemblyNames.Any(an => string.Equals(p.Name, an, StringComparison.InvariantCultureIgnoreCase)));
        }

        private async Task<NpmPackage> CompileTsAsync(IEnumerable<NpmPackage> packages, string assemblyName, string ts)
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
            });

            if (nodeRes.ExitCode != 0)
            {
                throw new Exception("NodeError:" + nodeRes.Out);
            }

            var files = new List<NpmPackageFile>();
            foreach(var resFile in nodeRes.ResultFiles)
            {
                var content = await ReadContent(resFile.Content);
                files.Add(new NpmPackageFile() { Content = content, FilePath = resFile.FileName });
            }
            return new NpmPackage()
            {
                Name = assemblyName.ToLower(),
                Files = files
            };
        }

        private async Task<string> ReadContent(Stream content)
        {
            using (var sr = new StreamReader(content))
            {
                return await sr.ReadToEndAsync();
            }
        }

        public async Task<FileContent> CreateNpmZip(IEnumerable<string> assemblyNames, CancellationToken cancellationToken = default)
        {
            var packages = await CreateNpmPackage(assemblyNames, cancellationToken);
            var ms = new MemoryStream();
            using (var zo = new ZipOutputStream(ms))
            {
                zo.SetLevel(9);
                foreach (var package in packages)
                {
                    foreach (var f in package.Files)
                    {
                        var entry = new ZipEntry($"{package.Name}/{f.FilePath}");
                        zo.PutNextEntry(entry);
                        var arr = Encoding.UTF8.GetBytes(f.Content);
                        await zo.WriteAsync(arr, 0, arr.Length);
                    }
                }
            }
            return new FileContent()
            {
                Content = new MemoryStream(ms.ToArray()),
                ContentType = "application/zip"
            };
        }
    }
}
