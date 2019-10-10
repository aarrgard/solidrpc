using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidRpc.OpenApi.Model.Json.Impl
{
    /// <summary>
    /// Represents a Json array.
    /// </summary>
    public class JsonArray<T> : JsonStruct, IJsonArray where T:IJsonStruct
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        public JsonArray(IJsonStruct parent) : base(parent)
        {
            Elements = new List<T>();
        }

        private IList<T> Elements { get; }

        public int Count => Elements.Count;

        public bool IsReadOnly => Elements.IsReadOnly;

        public IJsonStruct this[int index] { get => Elements[index]; set => Elements[index] = (T)value; }

        /// <summary>
        /// Returns the enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IJsonStruct> GetEnumerator()
        {
            return Elements.OfType<IJsonStruct>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddStruct(T s)
        {
            Elements.Add(s);
        }

        public int IndexOf(IJsonStruct item)
        {
            return Elements.IndexOf((T)item);
        }

        public void Insert(int index, IJsonStruct item)
        {
            Elements.Insert(index, (T)item);
        }

        public void RemoveAt(int index)
        {
            Elements.RemoveAt(index);
        }

        public void Add(IJsonStruct item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            Elements.Clear();
        }

        public bool Contains(IJsonStruct item)
        {
            return Elements.Contains((T)item);
        }

        public void CopyTo(IJsonStruct[] array, int arrayIndex)
        {
            Array.Copy(Elements.ToArray(), 0, array, 0, Elements.Count);
        }

        public bool Remove(IJsonStruct item)
        {
            return Elements.Remove((T)item);
        }
    }
}
