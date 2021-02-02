using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Model
{
    /// <summary>
    /// The vase class for the model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class ModelBaseDynamic<T> : ModelBase, IDictionary<string, T>
    {
        private readonly IDictionary<string, T> _container;

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        public ModelBaseDynamic(ModelBase parent) : base(parent)
        {
            _container = new Dictionary<string, T>();
        }

        /// <summary>
        /// Returns the value for supplied key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T this[string key]
        {
            get
            {
                if(_container.TryGetValue(key, out T res)) {
                    return res;
                }
                return default(T);
            }
            set
            {
                _container[key] = value;
            }
        }

        /// <summary>
        /// Returns all the keys
        /// </summary>
        [IgnoreDataMember]
        public ICollection<string> Keys => _container.Keys;

        /// <summary>
        /// Returns all the values
        /// </summary>
        [IgnoreDataMember]
        public ICollection<T> Values => _container.Values;

        /// <summary>
        /// The number of values
        /// </summary>
        [IgnoreDataMember]
        public int Count => _container.Count;

        /// <summary>
        /// Is the entry read only
        /// </summary>
        [IgnoreDataMember]
        public bool IsReadOnly => _container.IsReadOnly;

        /// <summary>
        /// Adds a new value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, T value)
        {
            _container.Add(key, value);
        }

        /// <summary>
        /// Adds a new value
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, T> item)
        {
            _container.Add(item);
        }

        /// <summary>
        /// Clears all values
        /// </summary>
        public void Clear()
        {
            _container.Clear();
        }

        /// <summary>
        /// Returns true if supplied value is a member
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, T> item)
        {
            return _container.Contains(item);
        }

        /// <summary>
        /// Returns true if the key is a member
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _container.ContainsKey(key);
        }

        /// <summary>
        /// Copies the values
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            _container.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns all the values
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return _container.GetEnumerator();
        }

        /// <summary>
        /// Removes element for supplied key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return _container.Remove(key);
        }

        /// <summary>
        /// Removes supplied item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, T> item)
        {
            return _container.Remove(item);
        }

        /// <summary>
        /// Tries to get the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out T value)
        {
            return _container.TryGetValue(key, out value);
        }

        /// <summary>
        /// Returns all the values
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _container.GetEnumerator();
        }
    }
}
