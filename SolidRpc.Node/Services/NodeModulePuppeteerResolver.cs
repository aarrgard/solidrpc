using SolidRpc.Abstractions;
using SolidRpc.Node.Services;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(INodeModuleResolver), typeof(NodeModulePuppeteerResolver), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.Node.Services
{
    /// <summary>
    /// Implements the node service.
    /// </summary>
    public class NodeModulePuppeteerResolver : INodeModuleResolver
    {
        public const string StrModuleId = "b0b6eacc-2766-47ac-9b48-68e68645314b";
        public static readonly Guid GuidModuleId = Guid.Parse(StrModuleId);

        public NodeModulePuppeteerResolver(INodeService nodeService)
        {
            NodeService = nodeService;
        }
        public Guid ModuleId => GuidModuleId;
        private INodeService NodeService { get; }

        public async Task<string> ExplodeNodeModulesAsync(DirectoryInfo directoryInfo, CancellationToken cancellationToken = default)
        {
            var packageFile = new FileInfo(Path.Combine(directoryInfo.FullName, "package.json"));
            using (var tw = packageFile.CreateText())
            {
                tw.Write(@"{
""dependencies"": {
    ""puppeteer"": ""^7.1.0""
  }
}");
            }
            await NodeService.ExecuteFileAsync(NodeModuleNpmResolver.GuidModuleId, directoryInfo.FullName, "npm\\bin\\npm-cli.js", new[] { "install" }, cancellationToken);
            return Path.Combine(directoryInfo.FullName, "node_modules");
        }
    }
}
