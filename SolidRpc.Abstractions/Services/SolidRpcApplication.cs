using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcAbstractionProviderAttribute(typeof(ISolidRpcApplication), typeof(SolidRpcApplication))]
namespace SolidRpc.Abstractions.Services
{
    public class SolidRpcApplication : ISolidRpcApplication
    {
        private IList<Task> _startupTasks = new List<Task>();
        public CancellationToken ShutdownToken => CancellationToken.None;

        public void AddStartupTask(Task startupTask)
        {
            lock (_startupTasks)
            {
                _startupTasks.Add(startupTask);
            }
        }

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
