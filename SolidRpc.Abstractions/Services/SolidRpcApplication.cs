using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcServiceAttribute(typeof(ISolidRpcApplication), typeof(SolidRpcApplication))]
namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Implements the solid application logic
    /// </summary>
    public class SolidRpcApplication : ISolidRpcApplication
    {
        private IList<Task> _startupTasks = new List<Task>();

        /// <summary>
        /// Returns the shutdown token
        /// </summary>
        public CancellationToken ShutdownToken => CancellationToken.None;

        /// <summary>
        /// Adds a startup task
        /// </summary>
        /// <param name="startupTask"></param>
        public void AddStartupTask(Task startupTask)
        {
            lock (_startupTasks)
            {
                _startupTasks.Add(startupTask);
            }
        }

        /// <summary>
        /// Invoked to wait for all the startup tasks to complete.
        /// </summary>
        /// <returns></returns>
        public Task WaitForStartupTasks()
        {
            lock(_startupTasks)
            {
                if (!_startupTasks.Any())
                {
                    return Task.CompletedTask;
                }
                for(int i = 0; i < _startupTasks.Count;)
                {
                    if(_startupTasks[i].IsCompleted)
                    {
                        _startupTasks.Remove(_startupTasks[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
                return Task.WhenAll(_startupTasks);
            }
        }
    }
}
