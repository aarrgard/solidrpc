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
        /// Returns a token that will be cancelled when the application shuts down.
        /// </summary>
        CancellationToken ShutdownToken { get; }

        /// <summary>
        /// Adds a startup task that we should wait for before starting to interact with the application.
        /// </summary>
        /// <param name="startupTask"></param>
        void AddStartupTask(Task startupTask);

        /// <summary>
        /// Ensures that all the startup tasks have completed.
        /// </summary>
        /// <returns></returns>
        Task WaitForStartupTasks();

    }
}
