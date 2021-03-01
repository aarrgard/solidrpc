using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Node.InternalServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(INodeProcessFactory), typeof(NodeProcessFactory), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.Node.InternalServices
{
    /// <summary>
    /// Factory for creating node processes.
    /// </summary>
    public class NodeProcessFactory : INodeProcessFactory
    {
        
        public NodeProcessFactory(
            ILogger<NodeProcessFactory> logger, 
            INodeModulesRepository nodeModulesRepository,
            ISerializerFactory serializerFactory,
            IEnumerable<INodePath> nodePaths)
        {
            Logger = logger;
            NodeModulesRepository = nodeModulesRepository;
            SerializerFactory = serializerFactory;
            NodeExePath = nodePaths.SelectMany(o => o.GetNodeExePaths()).FirstOrDefault() ?? throw new Exception("Cannot find node.exe");
            AvailableProcesses = new ConcurrentDictionary<Guid, IList<NodeProcess>>();
        }

        internal ILogger Logger { get; }
        internal ISerializerFactory SerializerFactory { get; }
        private string NodeExePath { get; }
        private INodeModulesRepository NodeModulesRepository { get; }
        private ConcurrentDictionary<Guid, IList<NodeProcess>> AvailableProcesses { get; }

        public async Task<INodeProcess> CreateNodeProcessAsync(Guid modulesId, string workingDir, CancellationToken cancellationToken)
        {
            var processes = AvailableProcesses.GetOrAdd(modulesId, _ => new List<NodeProcess>());
            NodeProcess np;
            lock(processes)
            {
                np = processes.FirstOrDefault();
                processes.Remove(np);
            }
            if(np != null)
            {
                if(await np.IsAlive(cancellationToken))
                {
                    return np;
                }
                else
                {
                    CleanupNodeProcess(np);
                }
            }
            if (workingDir == null)
            {
                workingDir = GetNodeWorkingDir();
            }
            var nodeModulesDir = await NodeModulesRepository.GetNodeModulePathAsync(modulesId);
            var nodecontext = new NodeContext(
                modulesId,
                NodeExePath,
                workingDir,
                nodeModulesDir
                );
            np = new NodeProcess(this, nodecontext);
            return np;
        }

        private string GetNodeWorkingDir()
        {
            var uid = Guid.NewGuid().ToString();
            var workingDir = Path.Combine(Path.GetTempPath(), uid);
            Directory.CreateDirectory(workingDir);
            return workingDir;
        }

        internal void ReturnNodeProcess(NodeProcess nodeProcess)
        {
            var processes = AvailableProcesses.GetOrAdd(nodeProcess.Scope, _ => new List<NodeProcess>());
            lock (processes)
            {
                processes.Add(nodeProcess);
            }
        }

        private async void CleanupNodeProcess(NodeProcess nodeProcess)
        {
            //
            // cleanup
            //
            await nodeProcess.Kill();
            var workingDir = nodeProcess.NodeWorkingDir;
            while (Directory.Exists(workingDir))
            {
                try
                {
                    Directory.Delete(workingDir, true);
                }
                catch
                {
                    await Task.Delay(100);
                }
            }

        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              