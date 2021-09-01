using SolidRpc.Abstractions.OpenApi.Invoker;

namespace SolidRpc.OpenApi.Binder.Invoker
{
    /// <summary>
    /// A queue bus message.
    /// </summary>
    public class MemoryQueueBusMessage
    {
        /// <summary>
        /// The message
        /// </summary>
        public string Message { get; internal set; }

        /// <summary>
        /// The options
        /// </summary>
        public InvocationOptions Options { get; internal set; }
    }
}