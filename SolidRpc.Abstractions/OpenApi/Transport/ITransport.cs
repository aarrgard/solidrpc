using SolidRpc.Abstractions.OpenApi.Binder;
using System;

namespace SolidRpc.Abstractions.OpenApi.Transport
{
    /// <summary>
    /// Base definitions for a transport
    /// </summary>
    public interface ITransport
    {
        /// <summary>
        /// The transport type. 
        /// </summary>
        string TransportType { get; }

        /// <summary>
        /// The operation address
        /// </summary>
        Uri OperationAddress { get; }

        /// <summary>
        /// Configures the transport for supplied binding.
        /// </summary>
        /// <param name="methodBinding"></param>
        void Configure(IMethodBinding methodBinding);
    }
}
