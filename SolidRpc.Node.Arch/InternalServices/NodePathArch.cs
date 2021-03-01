using SolidRpc.Abstractions;
using SolidRpc.Node.Arch.InternalServices;
using SolidRpc.Node.InternalServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[assembly: SolidRpcService(typeof(INodePath), typeof(NodePathArch), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.Node.Arch.InternalServices
{
    public class NodePathArch : INodePath
    {
        public IEnumerable<string> GetNodeExePaths()
        {
            //
            // determine executable
            //
            var arch = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            var dllLocation = new FileInfo(GetType().Assembly.Location);
            string executableName;
            switch (arch)
            {
                case "x86":
                case "AMD64":
                    executableName = "node.exe";
                    break;
                default:
                    throw new Exception($"Cannot handle architecture {arch}");
            }
            var exeFileNames = new string[] {
                    Path.Combine(dllLocation.Directory.FullName, "Arch", arch, executableName),
                    Path.Combine(dllLocation.Directory.Parent.FullName, "Arch", arch, executableName),
                };
            var exeFileName = exeFileNames.FirstOrDefault(o => File.Exists(o));
            if (exeFileName == null)
            {
                throw new Exception($"Cannot find any of the executables {string.Join(",", exeFileNames)}");
            }
            return new string[] { exeFileName };
        }
    }
}
