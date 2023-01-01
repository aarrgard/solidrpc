
namespace System.Collections.Generic
{ 
    /// <summary>
    /// Extension methods for an enumeration
    /// </summary>
    public static class IEnumerationExtensions
    {
        /// <summary>
        /// Returns an empty array if supplied argument is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static IEnumerable<T> AsNotNull<T>(this IEnumerable<T> e)
        {
            if (e == null) return new T[0];
            return e;
        }
    }
}
