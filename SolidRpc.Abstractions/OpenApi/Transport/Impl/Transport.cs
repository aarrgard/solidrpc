using SolidRpc.Abstractions.OpenApi.Binder;
using System;

namespace SolidRpc.Abstractions.OpenApi.Transport.Impl
{
    /// <summary>
    /// Base type for the transports
    /// </summary>
    public abstract class Transport : ITransport
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="transportType"></param>
        /// <param name="invocationStrategy"></param>
        public Transport(string transportType, InvocationStrategy invocationStrategy)
        {
            TransportType = transportType ?? throw new ArgumentNullException(nameof(transportType));
            InvocationStrategy = invocationStrategy;
        }

        /// <summary>
        /// The transport type.
        /// </summary>
        public string TransportType { get; }

        /// <summary>
        /// Returns the operation address
        /// </summary>
        public abstract Uri OperationAddress { get; }

        /// <summary>
        /// Returns the invocation strategy
        /// </summary>
        public InvocationStrategy InvocationStrategy { get; }

        /// <summary>
        /// Method that can be overridden to configure a transport.
        /// </summary>
        /// <param name="methodBinding"></param>
        public virtual void Configure(IMethodBinding methodBinding)
        {
        }
    }
}
