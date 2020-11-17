using SolidRpc.Abstractions;
using SolidRpc.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcServiceAttribute(typeof(ISolidRpcApplication), typeof(SolidRpcApplication), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Implements the solid application logic
    /// </summary>
    public class SolidRpcApplication : ISolidRpcApplication, IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private IList<Task> _startupTasks = new List<Task>();
        private IList<Func<Task>> _startupCallbacks = new List<Func<Task>>();
        private IList<Func<Task>> _shutdownCallbacks = new List<Func<Task>>();
        private Guid _hostId = Guid.NewGuid();

        /// <summary>
        /// Returns the shutdown token
        /// </summary>
        public CancellationToken ShutdownToken => _cancellationTokenSource.Token;

        /// <summary>
        /// The host id
        /// </summary>
        public Guid HostId => _hostId;

        /// <summary>
        /// Adds a shutdown callback
        /// </summary>
        /// <param name="shutdownCallback"></param>
        public void AddShutdownCallback(Func<Task> shutdownCallback)
        {
            _shutdownCallbacks.Add(shutdownCallback);
        }

        /// <summary>
        /// Adds a startup callback
        /// </summary>
        /// <param name="startupCallback"></param>
        public void AddStartupCallback(Func<Task> startupCallback)
        {
            _startupCallbacks.Add(startupCallback);
        }

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
        /// Dispses the application
        /// </summary>
        public void Dispose()
        {
            StopApplication();
        }

        /// <summary>
        /// Stops the application
        /// </summary>
        public void StopApplication()
        {
            _cancellationTokenSource.Cancel();
            foreach(var shutdownCallback in _shutdownCallbacks)
            {
                shutdownCallback();
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
                if (!_startupTasks.Any() && !_startupCallbacks.Any())
                {
                    return Task.CompletedTask;
                }
                foreach (var startupCallback in _startupCallbacks)
                {
                    _startupTasks.Add(startupCallback());
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
