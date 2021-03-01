using SolidRpc.Node.InternalServices;
using SolidRpc.Node.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Node.Services
{
    /// <summary>
    /// Implements the node service.
    /// </summary>
    public class NodeService : INodeService
    {
        private static Guid s_DefaultScope = Guid.Empty;

         /// <summary>
         /// Constructs the instance
         /// </summary>
         /// <param name="logger"></param>
        public NodeService(
            INodeProcessFactory nodeProcessFactory,
            INodeModulesRepository nodeModulesRepository)
        {
            NodeProcessFactory = nodeProcessFactory;
            NodeModulesRepository = nodeModulesRepository;
        }

        private INodeProcessFactory NodeProcessFactory { get; }

        private INodeModulesRepository NodeModulesRepository { get; }

        public async Task<string> GetNodeVersionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var np = await NodeProcessFactory.CreateNodeProcessAsync(s_DefaultScope, null, cancellationToken))
            {
                return await np.GetVersionAsync(cancellationToken);
            }
        }

        public Task<IEnumerable<NodeModules>> ListNodeModulesAsync(CancellationToken cancellationToken = default)
        {
            return NodeModulesRepository.GetNodeModulesAsync(cancellationToken);
        }

        public async Task<NodeExecution> ExecuteScriptAsync(Guid moduleId, string js, CancellationToken cancellationToken = default)
        {
            using (var np = await NodeProcessFactory.CreateNodeProcessAsync(moduleId, null, cancellationToken))
            {
                return await np.ExecuteScriptAsync(js, cancellationToken);
            }
        }

        public async Task<NodeExecution> ExecuteFileAsync(Guid moduleId, string workingDir, string fileName, IEnumerable<string> args, CancellationToken cancellationToken = default)
        {
            using (var np = await NodeProcessFactory.CreateNodeProcessAsync(moduleId, workingDir, cancellationToken))
            {
                return await np.ExecuteFileAsync(fileName, args, cancellationToken);
            }
        }
    }
}
