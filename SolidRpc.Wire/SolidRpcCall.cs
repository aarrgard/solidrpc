using System;
using System.Collections.Generic;

namespace SolidRpc.Wire
{
    /// <summary>
    /// Represents a solid rpc call. The call structure contains enough information
    /// to invoke a method on an interface.
    /// </summary>
    public class SolidRpcCall
    {
        /// <summary>
        /// The metadata for the method we are invoking.
        /// </summary>
        public SolidRpcMethodInfo MethodInfo { get; set; }

        /// <summary>
        /// The arguments
        /// </summary>
        public IEnumerable<SoldRpcCallArg> Arguments { get; set; }
    }
}
