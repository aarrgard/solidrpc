using SolidRpc.Abstractions;
using SolidRpc.Node.InternalServices;
using SolidRpc.Node.Services;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(INodeModuleResolver), typeof(NodeModuleRpcResolver), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.Node.Services
{
    /// <summary>
    /// Implements the node service.
    /// </summary>
    public class NodeModuleRpcResolver : INodeModuleResolver
    {
        public const string StrModuleId = "e29485f2-9392-4d1a-b19a-9b31091c19b4";
        public static readonly Guid GuidModuleId = Guid.Parse(StrModuleId);

        public static string tsconfig = $@"{{
  ""include"": [ ""**/*.ts"" ],
  ""exclude"": [ ""node_modules"", ""dist"" ],
  ""compilerOptions"": {{
    ""types"": [""node""],
    ""module"": ""commonjs"",
    ""target"": ""es6"",
    ""strict"": true,
    ""esModuleInterop"": true,
    ""baseUrl"": ""."",
    ""paths"": {{ ""*"": [ ""node_modules/*"",""../{StrModuleId}/node_modules/*"" ] }},
    ""typeRoots"": [ ""../{StrModuleId}/node_modules/@types"" ],
    ""lib"": [ ""es2018"",""es2017"",""es7"",""es6"",""DOM"" ],
    ""declaration"": true,
    ""sourceMap"": true
  }}
}}";

        public NodeModuleRpcResolver(INodeService nodeService)
        {
            NodeService = nodeService;
        }
        public Guid ModuleId => GuidModuleId;
        private INodeService NodeService { get; }
        private DirectoryInfo BaseDir { get; set; }

        public async Task ExplodeNodeModulesAsync(DirectoryInfo directoryInfo, CancellationToken cancellationToken = default)
        {
            BaseDir = directoryInfo;
            var explodedFile = new FileInfo(Path.Combine(directoryInfo.FullName, "exploded"));
            if (explodedFile.Exists) return;

            var packageFile = new FileInfo(Path.Combine(directoryInfo.FullName, "package.json"));
            using (var tw = packageFile.CreateText())
            {
                tw.Write(@"{
 ""dependencies"": {
    ""@types/node"": ""^18.14.2"",
    ""open"": ""^8.4.2"",
    ""axios"": ""^1.3.4"",
    ""cancellationtoken"": ""^2.2.0"",
    ""rxjs"": ""^7.8.0"",
    ""typescript"": ""^4.9.5""
 }
}");
            }
            var sep = Path.DirectorySeparatorChar;
            var res = await NodeService.ExecuteFileAsync(NodeModuleNpmResolver.GuidModuleId, directoryInfo.FullName, $"npm{sep}bin{sep}npm-cli.js", new[] { "install" }, cancellationToken);
            if(res.ExitCode != 0)
            {
                throw new Exception("Cannot explode module. 'npm install' failed.");
            }
            using (var fs = explodedFile.CreateText())
            {

            }
        }

        public async Task SetupWorkDirAsync(string nodeModulesDir, string workDir, CancellationToken cancellationToken = default)
        {
            var tsconfigFile = new FileInfo(Path.Combine(workDir, "tsconfig.json"));
            if (!tsconfigFile.Exists)
            {
                using (var w = tsconfigFile.CreateText())
                {
                    await w.WriteAsync(tsconfig);
                }
            }

            // copy package.json file
            var packageFile = new FileInfo(Path.Combine(workDir, "package.json"));
            var srcPackageFile = new FileInfo(Path.Combine(BaseDir.FullName, "package.json"));
            srcPackageFile.CopyTo(packageFile.FullName, true);
        }
    }
}
