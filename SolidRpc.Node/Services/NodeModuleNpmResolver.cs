using SolidRpc.Abstractions;
using SolidRpc.Node.Services;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(INodeModuleResolver), typeof(NodeModuleNpmResolver), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.Node.Services
{
    /// <summary>
    /// Implements the node service.
    /// </summary>
    public class NodeModuleNpmResolver : INodeModuleResolver
    {
        public const string StrModuleId = "83d3fb56-0d08-4506-ac9a-0a4373795350";
        public static readonly Guid GuidModuleId = Guid.Parse(StrModuleId);
        public Guid ModuleId => GuidModuleId;

        public async Task<string> ExplodeNodeModulesAsync(DirectoryInfo directoryInfo, CancellationToken cancellationToken = default)
        {
            var a = GetType().Assembly;
            var resName = a.GetManifestResourceNames().Single(o => o.EndsWith($"{StrModuleId}.zip"));
            var stream = a.GetManifestResourceStream(resName);

            using (var za = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                foreach (var ze in za.Entries)
                {
                    if (string.IsNullOrEmpty(ze.Name)) continue;
                    var destFile = new FileInfo(Path.Combine(directoryInfo.FullName, ze.FullName));
                    if (destFile.Exists)
                    {
                        if (destFile.Length == ze.Length)
                        {
                            continue;
                        }
                    }
                    CreateDirectory(destFile.Directory);
                    using (var fs = destFile.Open(FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (var zs = ze.Open())
                        {
                            await zs.CopyToAsync(fs);
                        }
                    }
                }
            }
            return directoryInfo.FullName;
        }

        private void CreateDirectory(DirectoryInfo directory)
        {
            if (directory.Exists) return;
            CreateDirectory(directory.Parent);
            directory.Create();
        }
    }
}
