using System.Collections.Generic;

namespace System
{
    internal interface IEnumerable<T1, T2>
    {
        IDictionary<string, string> Select(Func<object, object> p);
    }
}