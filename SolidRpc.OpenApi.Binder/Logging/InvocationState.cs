using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.Binder.Logger
{
    /// <summary>
    /// The logger scope is added to the logger when invocations are picked up by the queue handler.
    /// </summary>
    public class InvocationState : IEnumerable<KeyValuePair<string, object>>
    {
        private IEnumerable<KeyValuePair<string, object>> kvps;

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public InvocationState()
        {
            kvps = new KeyValuePair<string, object>[]
            {  
                new KeyValuePair<string, object>(nameof(SolidRpcCallId), Guid.NewGuid().ToString())
            };
        }

        /// <summary>
        /// The invocation id.
        /// </summary>
        public string SolidRpcCallId => (string)kvps.First().Value;

       IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return kvps.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return kvps.GetEnumerator();
        }
    }
}
