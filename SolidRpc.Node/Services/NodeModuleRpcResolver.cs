﻿using SolidRpc.Abstractions;
using SolidRpc.Node.Services;
using System;
using System.IO;
using System.Linq;
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

        public NodeModuleRpcResolver(INodeService nodeService)
        {
            NodeService = nodeService;
        }
        public Guid ModuleId => GuidModuleId;
        private INodeService NodeService { get; }

        public async Task ExplodeNodeModulesAsync(DirectoryInfo directoryInfo, CancellationToken cancellationToken = default)
        {
            var packageFile = new FileInfo(Path.Combine(directoryInfo.FullName, "package.json"));
            using (var tw = packageFile.CreateText())
            {
                tw.Write(@"{
""dependencies"": {
    ""axios"": ""^0.21.1"",
    ""cancellationtoken"": ""^2.2.0"",
    ""qs"": ""^6.10.1"",
    ""rxjs"": ""^7.1.0"",
    ""typescript"": ""^4.3.4""
  }
}");
            }
            var sep = Path.DirectorySeparatorChar;
            var res = await NodeService.ExecuteFileAsync(NodeModuleNpmResolver.GuidModuleId, directoryInfo.FullName, $"npm{sep}bin{sep}npm-cli.js", new[] { "install" }, cancellationToken);
            if(res.ExitCode != 0)
            {
                throw new Exception("Cannot explode module!");
            }
        }

        public async Task SetupWorkDirAsync(string nodeModulesDir, string workDir, CancellationToken cancellationToken = default)
        {
            var tsconfig = new FileInfo(Path.Combine(workDir, "tsconfig.json"));
            if(!tsconfig.Exists)
            {
                var nodeModulesPath = string.Join("/", nodeModulesDir.Split(Path.DirectorySeparatorChar).Reverse().Take(2).Reverse());
                using (var w = tsconfig.CreateText())
                {
                    await w.WriteAsync($@"{{
  ""include"": [ ""**/*.ts"" ],
  ""exclude"": [ ""node_modules"", ""dist"" ],
  ""compilerOptions"": {{
    ""baseUrl"": ""."",
    ""paths"": {{
      ""*"": [ ""../{nodeModulesPath}/*"" ],
      ""solidrpc"": [ ""solidrpc.js"" ]
    }},
    ""lib"": [ ""es2018"", ""DOM"" ]
  }}
}}");
                }
            }

        }
    }
}
