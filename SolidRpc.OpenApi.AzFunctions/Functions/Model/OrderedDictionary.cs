using System.Collections;
using System.Collections.Generic;

namespace SolidRpc.OpenApi.AzFunctions.Functions.Model
{
    internal class OrderedDictionary<TKey, TValue> : IDictionary<TKey,TValue>
    {
        private IDictionary<TKey, TValue> _backing;

        public OrderedDictionary(IDictionary<TKey, TValue> proxies)
        {
            _backing = proxies;
        }

        public TValue this[TKey key] { get => _backing[key]; set => _backing[key] = value; }

        public ICollection<TKey> Keys => _backing.Keys;

        public ICollection<TValue> Values => _backing.Values;

        public int Count => _backing.Count;

        public bool IsReadOnly => _backing.IsReadOnly;

        public void Add(TKey key, TValue value)
        {
            _backing.Add(key, value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _backing.Add(item);
        }

        public void Clear()
        {
            _backing.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _backing.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return _backing.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _backing.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _backing.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            return _backing.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _backing.Remove(item);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _backing.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _backing.GetEnumerator();
        }
    }
}