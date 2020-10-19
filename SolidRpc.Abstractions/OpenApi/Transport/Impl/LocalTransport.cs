using SolidRpc.Abstractions.OpenApi.Binder;
using System;

namespace SolidRpc.Abstractions.OpenApi.Transport.Impl
{
    /// <summary>
    /// Contains the settings for the queue transport.
    /// </summary>
    public class LocalTransport : Transport, ILocalTransport
    {
        private Uri _operationAddress;

        /// <summary>
        /// Represents a queue transport
        /// </summary>
        /// <param name="invocationStrategy"></param>
        public LocalTransport(InvocationStrategy invocationStrategy)
            : base("Local", invocationStrategy)
        {
        }

        public override Uri OperationAddress => _operationAddress;

        /// <summary>
        ///  configures this transport
        /// </summary>
        /// <param name="methodBinding"></param>
        public override void Configure(IMethodBinding methodBinding)
        {
        }
    }
}
