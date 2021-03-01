using SolidRpc.Abstractions;
using SolidRpc.Node.InternalServices;
using SolidRpc.Node.Services;
using SolidRpc.Node.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(INodeModulesRepository), typeof(NodeModuleRepository), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.Node.InternalServices
{
    public class NodeModuleRepository : INodeModulesRepository
    {
        
        
        public NodeModuleRepository(IEnumerable<INodeModuleResolver> resolvers)
        {
            Resolvers = resolvers;
            NodeModules = new ConcurrentDictionary<Guid, Task<string>>();
        }

        private IEnumerable<INodeModuleResolver> Resolvers { get; }
        private ConcurrentDictionary<Guid, Task<string>> NodeModules { get; }

        public Task<string> GetNodeModulePathAsync(Guid moduleId, CancellationToken cancellationToken = default)
        {
            return NodeModules.GetOrAdd(moduleId, _ => CreateNodeModule(_, cancellationToken));
        }

        private async Task<string> CreateNodeModule(Guid moduleId, CancellationToken cancellationToken)
        {
            var path = new DirectoryInfo(Path.Combine(Path.GetTempPath(), moduleId.ToString()));
            path.Create();

            foreach (var resolver in Resolvers)
            {
                if(resolver.ModuleId == moduleId)
                {
                    return await resolver.ExplodeNodeModulesAsync(path, cancellationToken);
                }
            }

            return path.FullName;
        }

        public Task<IEnumerable<NodeModules>> GetNodeModulesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
