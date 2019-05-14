using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SolidRpc.Swagger
{
    [DataContract]
    public class DynamicBase<T> : IDictionary<string, T>
    {
        private IDictionary<string, T> _container;

        public DynamicBase()
        {
            _container = new Dictionary<string, T>();
        }

        public T this[string key]
        {
            get
            {
                return _container[key];
            }
            set
            {
                _container[key] = value;
            }
        }

        [IgnoreDataMember]
        public ICollection<string> Keys => _container.Keys;

        [IgnoreDataMember]
        public ICollection<T> Values => _container.Values;

        [IgnoreDataMember]
        public int Count => _container.Count;

        [IgnoreDataMember]
        public bool IsReadOnly => _container.IsReadOnly;

        public void Add(string key, T value)
        {
            _container.Add(key, value);
        }

        public void Add(KeyValuePair<string, T> item)
        {
            _container.Add(item);
        }

        public void Clear()
        {
            _container.Clear();
        }

        public bool Contains(KeyValuePair<string, T> item)
        {
            return _container.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _container.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            _container.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return _container.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _container.Remove(key);
        }

        public bool Remove(KeyValuePair<string, T> item)
        {
            return _container.Remove(item);
        }

        public bool TryGetValue(string key, out T value)
        {
            return _container.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _container.GetEnumerator();
        }
    }
}
