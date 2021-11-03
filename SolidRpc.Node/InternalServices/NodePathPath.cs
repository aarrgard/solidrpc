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
            string exeName = "node.exe";
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    exeName = "node";
                    break;
                default:
                    exeName = "node.exe";
                    break;


            }
            foreach (var path in paths.Split(Path.PathSeparator))
            {
                var exePath = Path.Combine(path, exeName);
                if (File.Exists(exePath))
                {
                    return new string[] { exePath };
                }
            }
            return new string[0];
        }
    }
}
