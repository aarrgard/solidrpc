using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.Binder.Http
{
    public static class RequestDataExtensions
    {
        /// <summary>
        /// Returns the string data for supplied name
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<T> As<T>(this IEnumerable<HttpRequestData> data, string name)
        {
            return data.Where(o => o.Name == name).Select(o => o.As<T>());
        }
        /// <summary>
        /// Returns the binary data for supplied name
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetData<T>(this IEnumerable<HttpRequestData> data, string name)
        {
            return data.Single(o => o.Name == name).As<T>(); ;
        }
    }
}
