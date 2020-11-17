using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Services
{
    /// <summary>
    /// Implements some basic features of the application
    /// </summary>
    public interface ISolidRpcApplication
    {
        /// <summary>
        /// The host id - unique for each IoC container setup.
        /// </summary>
        Guid HostId { get; }

        /// <summary>
        /// Returns a token that will be cancelled when the application shuts down.
        /// </summary>
        CancellationToken ShutdownToken { get; }

        /// <summary>
        /// Adds a startup task that we should wait for before starting to interact with the application.
        /// </summary>
        /// <param name="startupTask"></param>
        void AddStartupTask(Task startupTask);

        /// <summary>
        /// Adds a shutdown callback
        /// </summary>
        /// <param name="startupCallback"></param>
        void AddStartupCallback(Func<Task> startupCallback);

        /// <summary>
        /// Adds a shutdown callback
        /// </summary>
        /// <param name="shutdownCallback"></param>
        void AddShutdownCallback(Func<Task> shutdownCallback);

        /// <summary>
        /// Ensures that all the startup tasks have completed.
        /// </summary>
        /// <returns></returns>
        Task WaitForStartupTasks();

        /// <summary>
        /// Stops this application
        /// </summary>
        void StopApplication();
    }
}
