using SolidRpc.Abstractions.OpenApi.Binder;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Abstractions.OpenApi.Transport.Impl
{
    /// <summary>
    /// Base type for the transports
    /// </summary>
    public class Transport : ITransport
    {
        /// <summary>
        /// Method that can be overridden to configure a transport.
        /// </summary>
        /// <param name="methodBinding"></param>
        public virtual void Configure(IMethodBinding methodBinding)
        {
        }
    }
}
