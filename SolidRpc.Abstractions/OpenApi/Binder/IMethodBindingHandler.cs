using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Binder
{
    /// <summary>
    /// Interface that can be implemted in order to receive callbacks when a method binding has been created.
    /// </summary>
    public interface IMethodBindingHandler
    {
        /// <summary>
        /// Invoked the the binding store has created a binding.
        /// </summary>
        /// <param name="binding"></param>
        void BindingCreated(IMethodBinding binding);

        /// <summary>
        /// Flushes the queues.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task FlushQueuesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
