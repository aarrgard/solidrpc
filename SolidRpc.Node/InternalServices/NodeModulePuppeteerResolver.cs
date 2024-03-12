using SolidRpc.Abstractions;
using SolidRpc.Node.InternalServices;
using SolidRpc.Node.Services;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(INodeModuleResolver), typeof(NodeModulePuppeteerResolver), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.Node.InternalServices
{
    /// <summary>
    /// Implements the node service.
    /// </summary>
    public class NodeModulePuppeteerResolver : INodeModuleResolver
    {
        public const string StrModuleId = "db58bdeb-49e4-484f-9598-2a327239738f";
        public static readonly Guid GuidModuleId = Guid.Parse(StrModuleId);

        public NodeModulePuppeteerResolver(INodeService nodeService)
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
    ""puppeteer"": ""^22.4.1""
  }
}");
            }
            var sep = Path.DirectorySeparatorChar;
            await NodeService.ExecuteFileAsync(NodeModuleNpmResolver.GuidModuleId, directoryInfo.FullName, $"npm{sep}bin{sep}npm-cli.js", new[] { "install" }, cancellationToken);
        }

        public Task SetupWorkDirAsync(string nodeModulesDir, string workDir, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
