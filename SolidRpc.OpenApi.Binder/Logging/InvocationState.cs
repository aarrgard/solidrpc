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
        public InvocationState(IEnumerable<KeyValuePair<string, object>> parentScope = null, string invocationId = null)
        {
            kvps = parentScope;
            InvocationId = kvps.Where(o => o.Key == nameof(InvocationId)).Select(o => o.Value).FirstOrDefault();
            if (!(kvps?.Any() ?? false))
            {
                kvps = (kvps ?? new KeyValuePair<string, object>[0]).Union(new KeyValuePair<string, object>[]
                {
                    new KeyValuePair<string, object>(nameof(InvocationId), invocationId ?? Guid.NewGuid().ToString())
                }).ToArray();
            }
        }

        /// <summary>
        /// The invocation id.
        /// </summary>
        public string InvocationId => (string)kvps.Where(o => o.Key ').First().Value;

        public IDictionary<string, string> Properties => this.Select(o => new { o.Key, Value = o.Value?.ToString() }).ToDictionary(o => o.Key, o => o.Value);

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
