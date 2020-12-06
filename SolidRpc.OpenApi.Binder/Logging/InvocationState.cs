using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.Binder.Logging
{
    /// <summary>
    /// The logger scope is added to the logger when invocations are picked up by the queue handler.
    /// </summary>
    public class InvocationState : IEnumerable<KeyValuePair<string, object>>
    {
        private static IEnumerable<KeyValuePair<string, object>> EmptyList = new KeyValuePair<string, object>[0];
        public static InvocationState EmptyState = new InvocationState(null, EmptyList);

        private InvocationState _parentState;
        private IEnumerable<KeyValuePair<string, object>> _scopeValues;
        private IEnumerable<string> _keys;

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public InvocationState(InvocationState parentState, IEnumerable<KeyValuePair<string, object>> scopeValues)
        {
            _parentState = parentState ?? EmptyState;
            _scopeValues = scopeValues ?? EmptyState;
            _keys = _scopeValues.Select(o => o.Key).ToList();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return (_parentState ?? EmptyList).Where(o => !_keys.Contains(o.Key)).Union(_scopeValues).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
