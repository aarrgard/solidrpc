using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Binder
{
    public static class RequestDataExtensions
    {
        /// <summary>
        /// Returns the string data for supplied name
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetStringData(this IEnumerable<HttpRequestData> data, string name)
        {
            return data.OfType<HttpRequestDataStrings>().Where(o => o.Name == name).SelectMany(o => o.StringData);
        }
        /// <summary>
        /// Returns the binary data for supplied name
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static byte[] GetBinaryData(this IEnumerable<HttpRequestData> data, string name)
        {
            return data.OfType<HttpRequestDataBinary>().Where(o => o.Name == name).Select(o => o.BinaryData).Single();
        }
    }
}
