using SolidRpc.Abstractions.OpenApi.Binder;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Abstractions.OpenApi.Transport
{
    /// <summary>
    /// Base definitions for a transport
    /// </summary>
    public interface ITransport
    {
        /// <summary>
        /// Configures the transport for supplied binding.
        /// </summary>
        /// <param name="methodBinding"></param>
        void Configure(IMethodBinding methodBinding);
    }
}
