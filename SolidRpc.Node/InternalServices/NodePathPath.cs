using SolidRpc.Abstractions;
using SolidRpc.Node.InternalServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

[assembly: SolidRpcService(typeof(INodePath), typeof(NodePathPath), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.Node.InternalServices
{
    public class NodePathPath : INodePath
    {
        public IEnumerable<string> GetNodeExePaths()
        {
            // look for node exe in any of the paths
            var paths = Environment.GetEnvironmentVariable("PATH");
            if (string.IsNullOrEmpty(paths))
            {
                throw new Exception("The environment does not contain a PATH entry");
            }
            foreach (var path in paths.Split(Path.PathSeparator))
            {
                var exePath = Path.Combine(path, "node.exe");
                if (File.Exists(exePath))
                {
                    return new string[] { exePath };
                }
            }
            return new string[0];
        }
    }
}
